   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	   9!clr-namespace:GAP.Custom_ControlsGAP.Custom_Controls  5   	   A =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL 5   	   "    GAP.DatasetImportMappingView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   3base"clr-namespace:GAP.ExtendedControls     5   	   0uc!clr-namespace:GAP.Custom_Controls     5   	   4BLL$clr-namespace:GAP.BL;assembly=GAP.BL     5   	   $	��690��5   	   $	��680��6      DatasetImportMappingView  � Title#  �   5   	     WindowDialog#��[ 5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    J� InputBindings 5      �� 5   
   
 �� Key$ I��67    �� 	Modifiers$ Controlf�6?    �� Command 6   �� *ImportCommand+6?    � 	Resources 5      %  GAP.BL.BoolToForeColorConverter  BoolToForegroundConverter��%   &	         5   
   6'   5      .��5      �  � RowDefinitions 5   
   �� 5        MinHeightRowDefinition#��[ 6   �� 5        MaxHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5        AutoHeightRowDefinition#��[ 6   �� 5      #��[ 6    � ColumnDefinitions 5   
   �� 5        MinWidthColumnDefinition#��[ 6   �� 5        AutoWidthColumnDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5        MaxWidthColumnDefinition#��[ 6   .+�5!   
   �� $��1��6   $��0 5 0 0q�6"   $	��Top=�64   $��1��6L   .+�5"      �� -   5        $��
lbDatasets��5"      $
��Left=�6(   $	��515��6C   $	��250��6P   	 �� SelectedItem	 6\   �� *SelectedDataset+��6�   �� *ImportedDatasetList+��5#      �� 5$      .R�5%      �� ��6$   �� *Dataset.Name+��6B   �� *
IsAccepted+
 �� UpdateSourceTrigger$
 PropertyChanged=� �� 	Converter# [ 5&      5(      5)      #   GAP.Custom_Controls.ucDataset  5*   
   $��1��6   $��	0 -10 0 0q�6$   $��3��68   $��3��6H   ]� 5+   
   $��3��6   $��Stretch=�6!   $��6��6?   $��0 0 10 0q�6S   .+�5,      � $��Stretch=�6    5-      �� 5.      #��[ 6+   �� 5/      #��[ 6+   �� 50      #��[ 6+   �� 51      #��[ 6+   �� 52      #��[ 6+   �� 53      #��[ 6+   .+�56      ��  � ToolTip$P JThis button will save all the unsaved datasets with the default properties��6   $��1��57      $��2��6-   $
��Left=�6A     	 SaveAllWithDefaultProperties#��� 	 58       
 MetroButton#��[
 6V   $	��320��59   J   ��6   �� *SaveAllPropertiesCommand+6J   �� 5:      $��3��6     ImportButton#���  6-   #��[
 6V   ��6{   �� *ImportCommand+�� 5;      $��5��6     CancelButton#���  6-   #��[
 6W   ��6|   �� *CancelCommand+5<      5=      5>      5?      