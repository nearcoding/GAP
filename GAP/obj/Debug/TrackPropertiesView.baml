   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	       GAP.TrackPropertiesView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   3base"clr-namespace:GAP.ExtendedControls     5   	      TrackProperties  � Title#  �   5   	   $	��250��63   $	��450��6@     WindowDialogResize#��[ 5   	    � WindowStartupLocation$ CenterScreen=�5   	    +� FocusedElement 5   	   ��  �� ElementName$ ComboBoxCharts�� � 	Resources 5
       �� Source�� 5   
   $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6   %    .��5      � $��10q�6    � ShowGridLines .   � RowDefinitions 5   
   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5        MaxHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6    � ColumnDefinitions 5   
   �� 5        AutoWidthColumnDefinition#��[ 6   �� 5        MinWidthColumnDefinition#��[ 6   �� 5        MaxWidthColumnDefinition#��[ 6   .+�5   
   ��   ChartThisTrack_TitleText#���  6   6H   �� 5   
    	 AccentColorBrush#��� 	 6   $��2��6D   $
��Left=�6T   $	��200��5      	 �� DisplayMemberPath$
	 Name��6>   ��6   �� *Charts+
 �� SelectedItem
 6W   �� *SelectedChart+�� 5   
   $��2��6    
 
TrackOrder#��� 
 6!   $��0 0 10 0q�6E   � 5    
   $��2��6   $��2��6    5!      �� 5"      #��[ 6#   �� 5#      #��[ 6#   �� 5$      #��[ 6#   �� 5%      #��[ 6#   .+�5(      �� -   5         ��Name$ ListBoxItems��5(   [   #��� 	 6   $��1q�6G   $
	 Name��6q   $��1��6�    �� SelectionMode$ Single=�5)      $��Stretch=�6i   ��5(   �   �� *Tracks+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�
 5)   5   �� *SelectedTrack+$ TwoWay=�6�   �� 5+      $��Center=�6   $��3��64   .+�5,      ��  # RoundButtonWithAccentBackground#��[ 6   $��35��6R   $��50��6^   $��	0 0 10 20q�6i    � ToolTip$ Up��6}   ��6�   �� *	UpCommand+��5-      �� 5.      $��../Images/UpArrow.png��6    50      �� 51      #��[ 6   $��35��6R   $��50��6^   $��0 0 10 0q�6i   $
 Down��6|   ��6�   �� *DownCommand+��52      �� 53      $��../Images/DownArrow.png��6    55      56      57      �� 59   
    �� Orientation$ 
Horizontal=�6   $��4��6.   $��2��6;   $��Right=�6K   .+�5:      ��   
ButtonSave#��[ 6   $��0 0 20 0q�69   6M   �� 5;        ButtonCancel#��[ 6   6=   5<      5=      5>      