   M S B A M L   `   `   `  ���� @  <GAP, Version=1.0.1.141, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls      GAP.AddCurveView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     3base"clr-namespace:GAP.ExtendedControls        AddCurve  � Title#  �   $	��250��$	��460��$	��400��$	��250��  WindowDialog#��[  � WindowStartupLocation$ CenterScreen=� � 	Resources  �� 
TargetType �� Property �� Value��%-   '��       �� " ��.���  � ��$ 23��.��� $��10q� � ColumnDefinitions ��   MaxWidthColumnDefinition#��[ ��   AutoWidthColumnDefinition#��[ ��   MinWidthColumnDefinition#��[ �� #��[ �� #��[  � RowDefinitions ��   MaxHeightRowDefinition#��[ ��   AutoHeightRowDefinition#��[ ��   MinHeightRowDefinition#��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ .+���   ChartThisCurve_TitleText#���  $��1��$��1���� -    ��Name$ 	cmbCharts��$��1��$��3��	 �� DisplayMemberPath$
	 Name������ *Charts+
 �� SelectedItem
 �� *SelectedChart+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=���  	 TrackThisCurve_TitleText#��� 	 $��3��$��1���� $��3��$
	 Name��$��3������ *Tracks+$ TwoWay=�$ PropertyChanged=�
 �� *SelectedTrack+$ TwoWay=�$ PropertyChanged=���  
 RelatedProject#��� 
 $��5��$��1���� $��5��$��3��$
	 Name������ *
Projects+
 �� *SelectedProject+$ TwoWay=�$ PropertyChanged=��� $��7��$��1��  RelatedWell#���  �� $��7��$��3��$
	 Name������ *Wells+
 �� *SelectedWell+$ TwoWay=�$ PropertyChanged=���   RelatedDataset#���  $��9��$��1���� $��9��$��3��$
	 Name������ *
Datasets+
 �� *SelectedDataset+$ TwoWay=�$ PropertyChanged=��� $��11��$��3��  SaveCancelButton#��[ 