   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	   A =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL 5   	       GAP.ZoomDialogView   5      +base"clr-namespace:GAP.ExtendedControls  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4bll$clr-namespace:GAP.BL;assembly=GAP.BL     5   	     � WindowStartupLocation$  CenterScreen=�5   
      WindowDialogResize#��[  6/     
ZoomTracks � Title# �  5   	   $	��400��6.   $	��400��6;    � 	Resources 5       �� MergedDictionaries �� Source�� 5	   
    5
      �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   %    5      .��5      � $��	0 10 0 10q�6    � RowDefinitions 5   
   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5        MaxHeightRowDefinition#��[ 6   �� 5        MicroHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6    � ColumnDefinitions 5   
   �� 5        MinWidthColumnDefinition#��[ 6   �� 5        AutoWidthColumnDefinition#��[ 6   �� 5       #��[ 6   �� 5!        MaxWidthColumnDefinition#��[ 6   �� 5"      #��[ 6   .+�5$   
   �� $��1��6    		 Chart#��� 	 6$   �� 5%   
   $
��Left=�6   $��3��6.   $	��200��6�    �� DisplayMemberPath$
 Name��6�   ��6>   �� *Charts+ �� SelectedItem 6]   �� *SelectedChart+	 �� Mode$	 TwoWay=�
 �� UpdateSourceTrigger$
 PropertyChanged=�6�   �� 5'   
   $��2��6   $��1��6!    	
 Track#��� 
 61   �� 5(   
     AccentColorBrush#���  61   $��1q�6b   $��3��6v   $��2��6�   ��6   �� *Tracks+��5)      �� 5*      .R�5+      ��  d� 	IsChecked 6   �� *IsEntitySelected+$	 TwoWay=�$
 PropertyChanged=���6w   �� *Name+5,      5.      �� 50   
   $��4��6   $��4��6!   �� 51   
   $��Center=�6   $��Min Zoom��6/   $��1��6?   $��6��6O   �� 52   
   $	��100��6   $
��Left=�6\   $��3��6w   $��6��6�   ��6   �� *
MinValue+$
 PropertyChanged=�6�   �� 54   
   $��Center=�6   $��Max Zoom��6/   $��1��6?   $��8��6O   �� 55   
   $	��100��6   $
��Left=�6\   $��8��6w   $��3��6�   ��6   �� *
MaxValue+$
 PropertyChanged=�6�   �� 57   
   $��10��6   $��4��6#   $��Right=�67    �� Orientation$ 
Horizontal=�6S   .+�58      �� $��Apply��6     MetroButton#��[ 6%   $	��120��6J   $��0 0 20 0q�6v   ��6V   �� *ZoomCommand+6v   �� 59        ButtonCancel#��[ 6   5=      5>      5?      