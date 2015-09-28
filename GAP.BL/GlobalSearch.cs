using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GAP.BL
{
    public class GlobalSearch
    {
        private GlobalSearch()
        {
            CheckPhysicalDirectory();
            CheckIndexFile();
        }

        public IEnumerable<HelpData> ExecuteQuery(string queryText)
        {
            Debug.WriteLine("Searching " + DateTime.Now.ToLongTimeString() + " This is query text " + queryText);
            if (string.IsNullOrWhiteSpace(queryText)) return new List<HelpData>();
            queryText = queryText.Trim() + "*";
            queryText = queryText.Replace(" ", " +");
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new[] { "BasicInfo", "Description", "ScreenName" }, analyzer);
            parser.DefaultOperator = QueryParser.Operator.AND;

            var query = parser.Parse(queryText);
            //against which indexSearcher we need to search
            var indexSearcher = new IndexSearcher(LuceneDirectory);
            var returnedRecords = indexSearcher.Search(query, 20);
            List<HelpData> lst = new List<HelpData>();
            for (int i = 0; i < returnedRecords.ScoreDocs.Length; i++)
            {
                var docID = returnedRecords.ScoreDocs[i].Doc;
                lst.Add(GetHelpDataByDocID(docID, indexSearcher));

            } return lst;
        }

        private HelpData GetHelpDataByDocID(int docID, IndexSearcher indexSearcher)
        {
            var document = indexSearcher.Doc(docID);
            return new HelpData
             {
                 BasicInfo = document.Get("BasicInfo"),
                 Description = document.Get("Description"),
                 ScreenName = document.Get("ScreenName"),
                 ViewName = document.Get("ViewName")
             };
        }

        private void CheckIndexFile()
        {
            if (!File.Exists(Path.Combine(_physicalDirectory, "_0.cfs")))
            {
                var results = new HelpDataRepository().GetDataForIndexing();
                AddSearchItemsToIndex(results);
            }
        }

        private void AddSearchItemToIndex(HelpData data, IndexWriter indexWriter)
        {
            var document = new Document();
            document.Add(new Field("BasicInfo", data.BasicInfo, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("Description", data.Description, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("ScreenName", data.ScreenName, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("ViewName", data.ViewName, Field.Store.YES, Field.Index.ANALYZED));
            indexWriter.AddDocument(document);
        }

        private void AddSearchItemsToIndex(IEnumerable<HelpData> data)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(LuceneDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var item in data)
                    AddSearchItemToIndex(item, writer);
                writer.Optimize();
                writer.Commit();
                analyzer.Close();
            }
        }

        FSDirectory _luceneDirectory;
        public FSDirectory LuceneDirectory
        {
            get
            {
                if (_luceneDirectory == null)
                    _luceneDirectory = FSDirectory.Open(_physicalDirectory);

                if (IndexWriter.IsLocked(_luceneDirectory))
                    IndexWriter.Unlock(_luceneDirectory);

                if (File.Exists(Path.Combine(_physicalDirectory, "writer.lock")))
                    File.Delete(Path.Combine(_physicalDirectory, "writer.lock"));

                return _luceneDirectory;
            }
        }
        private void GenerateIndexFile()
        {

        }

        string _physicalDirectory;
        
        private void CheckPhysicalDirectory()
        {
            _physicalDirectory = Path.Combine(HelperMethods.Instance.GetAppDataFolder(), "search_index");

            if (!System.IO.Directory.Exists(_physicalDirectory))
            {
                System.IO.Directory.CreateDirectory(Path.Combine(_physicalDirectory, "search_index"));
            }
        }

        static GlobalSearch _instance = new GlobalSearch();
        public static GlobalSearch Instance
        {
            get { return _instance; }
        }
    }//end class
}//end namespace
