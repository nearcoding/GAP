   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	       GAP.SelectDataset   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   3base"clr-namespace:GAP.ExtendedControls     5   	      WindowDialog#��[  5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ Select Dataset��5   	   $	��200��6    $	��350��6-   .��5	      � $��5q�6    � ColumnDefinitions 5
   
   �� 5        MaxWidthColumnDefinition#��[ 6   �� 5        AutoWidthColumnDefinition#��[ 6   �� 5        MinWidthColumnDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6    � RowDefinitions 5   
   �� 5        MaxHeightRowDefinition#��[ 6   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   .+�5   
   �� $��1��6   $��1��6!     Project#���  61   $��Center=�6R   �� 5   
   $��1��6   $��3��6     �� DisplayMemberPath$
 Name��6Q    �� SelectedValuePath$ ID��6j   ��60   �� *
Projects+ �� SelectedItem 6�   �� *SelectedProject+�� 5   
   $��1��6     Well#���  6%   $��3��6C   $��Center=�6P   �� 5    
   $��3��6   $��3��6    $
 Name��6N   $ ID��6g   ��60   �� *Wells+ 6~   �� *SelectedWell+�� 5"   
    	 Dataset#��� 	 6   $��1��66   $��5��6F   $��Center=�6S   �� 5#   
   $��5��6   $��3��6    $
 Name��6Q   $ ID��6j   ��60   �� *
Datasets+ 6�   �� *SelectedDataset+�� 5%   
   $��Right=�6   $��7��61   $��3��6>    �� Orientation$ 
Horizontal=�6N   .+�5&      �� $��Select  Dataset [Ctrl+S]��6    
 MetroButton#��[
 6a   ��68   �� *SelectDatasetCommand+6a   �� 5'      $��Cancel [Esc]��6   #��[
 6N   $��10 0 0 0q�6s   ��6,   �� *CancelCommand+6s   5(      5)      5*      