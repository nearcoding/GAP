   M S B A M L   `   `   `  ���� @  <GAP, Version=1.0.1.141, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  A =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL     GAP.ZoomDialogView   +base"clr-namespace:GAP.ExtendedControls  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     4bll$clr-namespace:GAP.BL;assembly=GAP.BL       � WindowStartupLocation$  CenterScreen=�   WindowDialogResize#��[    
ZoomTracks � Title# �  $	��400��$	��400�� � 	Resources  �� MergedDictionaries �� Source��  �� $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�%    .��� $��	0 10 0 10q� � RowDefinitions ��   AutoHeightRowDefinition#��[ ��   MinHeightRowDefinition#��[ ��   MaxHeightRowDefinition#��[ ��   MicroHeightRowDefinition#��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[  � ColumnDefinitions ��   MinWidthColumnDefinition#��[ ��   AutoWidthColumnDefinition#��[ �� #��[ ��   MaxWidthColumnDefinition#��[ �� #��[ .+��� $��1�� 		 Chart#��� 	 �� $
��Left=�$��3��$	��200�� �� DisplayMemberPath$
 Name������ *Charts+ �� SelectedItem �� *SelectedChart+	 �� Mode$	 TwoWay=�
 �� UpdateSourceTrigger$
 PropertyChanged=��� $��2��$��1�� 	
 Track#��� 
 ��   AccentColorBrush#���  $��1q�$��3��$��2������ *Tracks+���� .R���  d� 	IsChecked �� *IsEntitySelected+$	 TwoWay=�$
 PropertyChanged=����� *Name+�� $��4��$��4���� $��Center=�$��Min Zoom��$��1��$��6���� $	��100��$
��Left=�$��3��$��6������ *
MinValue+$
 PropertyChanged=��� $��Center=�$��Max Zoom��$��1��$��8���� $	��100��$
��Left=�$��8��$��3������ *
MaxValue+$
 PropertyChanged=��� $��10��$��4��$��Right=� �� Orientation$ 
Horizontal=�.+��� $��Apply��  MetroButton#��[ $	��120��$��0 0 20 0q����� *ZoomCommand+��   ButtonCancel#��[ 