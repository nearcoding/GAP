   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5          GAP.ImportDataView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      3base"clr-namespace:GAP.ExtendedControls     5         
ImportData  � Title#  �   5      $	��226��66   $	��660��6C     WindowDialog#��[ 5       � WindowStartupLocation$ CenterScreen=�5      .��5      � $��5q�6    � RowDefinitions 5	   
   �� 5
        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   .+�5   
   ��   ImportExportLabel#��[ 6   �� 5   
   $
��Bold"�6     ImportData_Alert#���  6&   
���  ��$��2��6a   $��Center=�6n   $��8(�6�   	� 5   
   -   5        $��rbExact��5        ImportData_ExactData#���  6a   $��Center=�6�   $��4��6�    d� 	IsChecked 6%   �� *IsImportExactDataChecked+ �� Mode$ TwoWay=�6�   	� 5   
   -   5        $��	rbAverage��5        ImportData_AverageData#���  6e   $��Center=�6�   $��6��6�    6'   �� *IsImportAverageDataChecked+$ TwoWay=�6�   �� 5   
   $��8��6    �� Orientation$ 
Horizontal=�6"   |�.  $��Right=�6M   .+�5      ��   
SaveButton#���  6    	 MetroButton#��[	 6<   $��0 0 20 0q�6�   ��6a   �� *ImportDataCommand+6�   �� 5       
 ButtonCancel#��[
 6   5      5      5       