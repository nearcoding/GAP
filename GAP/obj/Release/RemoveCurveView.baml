   M S B A M L   `   `   `  ���� @  <GAP, Version=1.0.1.141, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  A =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL     GAP.RemoveCurveView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     M IGAP.CustomControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+customhttp://nearcoding.com/controls  -base"clr-namespace:GAP.ExtendedControls  .BLL$clr-namespace:GAP.BL;assembly=GAP.BL    � WindowStartupLocation$  CenterScreen=�   RemoveCurves � Title# �   $	��520��$	��780�� � 	Resources #  GAP.BL.TrackIDToNameConverter  TrackIDToNameConverter#  GAP.BL.ChartIDToNameConverter  ChartIDToNameConverter"  GAP.BL.WellIDToNameConverter  WellIDToNameConverter%  GAP.BL.ProjectIDToNameConverter  ProjecIDToNameConverter%  GAP.BL.DatasetIDToNameConverter  DatasetIDToNameConverter��%K   &	       &	      &	 
     &	      &	                .��� $��5q� � RowDefinitions ��   AutoHeightRowDefinition#��[ ��   MinHeightRowDefinition#��[ ��   MaxHeightRowDefinition#��[ �� #��[ �� #��[ .+���  	 CurveRemove_TitleText#��� 	 $
��Bold"�)  #GAP.CustomControls.ExtendedDataGrid  &   System.Windows.Controls.DataGrid   CanUserSortColumns . $��2�� 
 MetroDataGrid#��[
 ���� *
CurvesList+   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,	  &System.Windows.Controls.DataGridColumn 	  Header ��  d� 	IsChecked ��  �� RelativeSource �� 	 �� AncestorType$	 WindowL�
 �� Path$$
 DataContext.AllRecordsSelected� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   CellTemplate �� .R��� $��Center=� �� *IsCurveSelected+$ TwoWay=�$ PropertyChanged=�0
  *System.Windows.Controls.DataGridTextColumn
   	  Width5  /System.Windows.Controls.DataGridLengthConverter$
 1.5* 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *DatasetCurveName+ �� 	Converter# [  ��   Curve/DatasetName#���  
  $ *  �� *
RefTrack+# [  ��   RefTrack#���  
  $ *  �� *
RefChart+# [  ��   RefChart#���  
  $ *  �� *	RefWell+# [  ��   RefWell#���  
  $ *  �� *
RefProject+# [  ��   
RefProject#���  �� $��4�� �� Orientation$ 
Horizontal=�$��Right=�.+���   MetroButton#��[ $��0 0 20 0q����� *SaveCommand+���� *SaveButtonText+$ PropertyChanged=� �� FallbackValue$ Save[Ctrl+S]����   ButtonCancel#��[ 