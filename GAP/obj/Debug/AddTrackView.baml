   M S B A M L   `   `   `  ����A  =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL  5   	   > :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls 5   	      GAP.AddTrackView   5      ,BLL$clr-namespace:GAP.BL;assembly=GAP.BL  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   3base"clr-namespace:GAP.ExtendedControls     5   	      AddTrack  � Title#  �   5   	   $	��150��6,   $	��470��69   $	��400��6E   $	��140��6T     WindowDialog#��[ 5   	    � WindowStartupLocation$ CenterScreen=�5
   	    +� FocusedElement 5	   	   ��  �� ElementName$ ComboBoxCharts�� � 	Resources 5        ComboWithoutStyle �� 
TargetType��%"   &	       �� 5   
   " ��6*   5      .��5      � $��10q�6    � ColumnDefinitions 5   
   �� 5        MaxWidthColumnDefinition#��[ 6   �� 5        AutoWidthColumnDefinition#��[ 6   �� 5        MinWidthColumnDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6    � RowDefinitions 5   
   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   .+�5   
   ��   ChartThisTrack_TitleText#���  6   $��1��6F   �� 5   
   -   5         ��Name$ ComboBoxCharts��5      #��[ 6+   $��0��5   G   $��3��6T   $	��500��6d   $��Stretch=�5       	 �� DisplayMemberPath$
	 Name��61   ��5   V   �� *Charts+
 �� SelectedItem
 5      �� *SelectedChart+ �� Mode$ TwoWay=�5    1   �� 5"   
    	 	TrackName#��� 	 6   $��2��67   $��1��6D   *  $GAP.ExtendedControls.TextBoxExtended  5#   
   $��2��6i   $��3��6v   $��Stretch=�6�   $	��200��6�   ��6    �� *CurrentObject.Name+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5%   
   $��4��6   $��3��6&    
 SaveCancelButton#��[
 66   5&      5'      