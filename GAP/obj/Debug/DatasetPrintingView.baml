   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	   A =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL 5   	       GAP.DatasetPrintingView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   3base"clr-namespace:GAP.ExtendedControls     5   	   4bll$clr-namespace:GAP.BL;assembly=GAP.BL     5   	      WindowDialogResize#��[  5   	     � WindowStartupLocation$  CenterScreen=�66   $	��400��5   !   $	��400��6.    � Title 6	   �� *Title+ � 	Resources 5       �� MergedDictionaries �� Source�� 5	   
    5
      �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   %    5      .��5      � $��5q�6    � RowDefinitions 5   
   �� 5        MaxHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5        AutoHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   .+�5   
   R� 
���   �$��0 0 0 2q�6'   $��Stretch=�6A   ��6f   �� *
Projects+ 5       �� DataType  GAP.BL.Project �� ItemsSource ��Name	 d� 	IsChecked
 �� Mode �� UpdateSourceTrigger
 � Tag �� RelativeSource �� Mode �� AncestorType  GAP.BL.Well  GAP.BL.Dataset��%U  (��       5   +   *, +)(��     5   +   *, +)(�� !    5!      *, +)�� 5      "  6+    6K   �� *Wells+.R�5      �� $ TextBlockProject��6�   $��Stretch=�6x   	 6   �� *IsEntitySelected+$
 TwoWay=�$ PropertyChanged=���6�   �� *Name+ 6�   �� *DataContext+ �� $ FindAncestor=�$ WindowL�5      �� 5      "  6+    6H   �� *
Datasets+.R�5      �� $��Stretch=�6x   	 6   �� *IsEntitySelected+$
 TwoWay=�$ PropertyChanged=���6�   �� *Name+ 6�   �� *DataContext+ �� $ FindAncestor=�$ WindowL�5       �� 5!      "  6   .R�5"      �� $��Stretch=�6x   	 6   �� *IsEntitySelected+$
 TwoWay=�$ PropertyChanged=���6�   �� *Name+ 6�   �� *DataContext+ �� $ FindAncestor=�$ WindowL�5#      5$      ��5%      �� 5&       �� 
TargetType" Q�6     MetroTreeViewItem �� BasedOn# [ 6;   .�5'      ��  �� Property �@Q�
IsExpanded �� Value . 63   5(      5*      �� 5+   
   $��2��6   � 5,   
   $��4��6    � ColumnDefinitions 5-      �� 5.        AutoWidthColumnDefinition#��[ 6#   �� 5/        MaxWidthColumnDefinition#��[ 6#   .+�51      ��   IncludeSpreadsheetData#���  6   	 52      �� *IncludeSpreadsheetData+$
 TwoWay=�$ PropertyChanged=��� 53        HyperLinkButtonStyle#��[ 6   $��Printer Settings��6C   $��1��54      $��Right=�6%   ��6A   �� *PrinterSettingsCommand+55      �� 57   
   $��6��6    �� Orientation$ 
Horizontal=�6#   $��Right=�6<   .+�58      ��  	 MetroButton#��[	 6    	
 Print#��� 
 6b   $��0 0 10 0q�6�   ��6:   �� *PrintDatasetCommand+6�   �� 59      #��[	 6     PrintPreview#���  6i   $��0 0 10 0q�6�   ��6:   �� *PrintPreviewDatasetCommand+6�   �� 5:      #��[	 6     CancelButton#���  6\   ��6:   �� *CancelCommand+6\   5;      5<      5=      