   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	   A =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL 5   	       GAP.RemoveCurveView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   M IGAP.CustomControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+customhttp://nearcoding.com/controls  5   	   -base"clr-namespace:GAP.ExtendedControls  5   	   .BLL$clr-namespace:GAP.BL;assembly=GAP.BL  5   	     � WindowStartupLocation$  CenterScreen=�5   9      RemoveCurves � Title# �   5   	   $	��520��60   $	��780��6=    � 	Resources 5      #  GAP.BL.TrackIDToNameConverter  TrackIDToNameConverter#  GAP.BL.ChartIDToNameConverter  ChartIDToNameConverter"  GAP.BL.WellIDToNameConverter  WellIDToNameConverter%  GAP.BL.ProjectIDToNameConverter  ProjecIDToNameConverter%  GAP.BL.DatasetIDToNameConverter  DatasetIDToNameConverter��%�   &	       &	      &	 &     &	 9     &	 L       5	   
   6%     5
   
   6%     5   
   6$     5   
   6'     5   
   6'   5      .��5      � $��5q�6    � RowDefinitions 5   
   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5        MaxHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   .+�5   
   ��  	 CurveRemove_TitleText#��� 	 6   $
��Bold"�6C   )  #GAP.CustomControls.ExtendedDataGrid  5   
   &   System.Windows.Controls.DataGrid   CanUserSortColumns . $��2��6<    
 MetroDataGrid#��[
 6I   ��6q   �� *
CurvesList+   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,	  &System.Windows.Controls.DataGridColumn 	  Header 5      �� 5       d� 	IsChecked 6#   ��  �� RelativeSource �� 	 �� AncestorType$	 WindowL�
 �� Path$$
 DataContext.AllRecordsSelected� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�      CellTemplate 5      �� 5      .R�5       �� $��Center=�6'    6D   �� *IsCurveSelected+$ TwoWay=�$ PropertyChanged=�5!      5#      0
  *System.Windows.Controls.DataGridTextColumn
  5%       	  Width5  /System.Windows.Controls.DataGridLengthConverter$
 1.5* 6~   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *DatasetCurveName+ �� 	Converter# [  5&      �� 5'        Curve/DatasetName#���  6$   5)      
  5*      $ * 6u    6%   �� *
RefTrack+# [  5+      �� 5,        RefTrack#���  6$   5.      
  5/      $ * 6t    6%   �� *
RefChart+# [  50      �� 51        RefChart#���  6$   53      
  54      $ * 6r    6%   �� *	RefWell+# [  55      �� 56        RefWell#���  6$   58      
  59      $ * 6x    6%   �� *
RefProject+# [  5:      �� 5;        
RefProject#���  6$   5=      5?      �� 5A   
   $��4��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5B      ��   MetroButton#��[ 65   $��0 0 20 0q�6Z   ��6   �� *SaveCommand+��6m   �� *SaveButtonText+$ PropertyChanged=� �� FallbackValue$ Save[Ctrl+S]��6�   �� 5C        ButtonCancel#��[ 6   5D      5E      5F      