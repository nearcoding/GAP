   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	       GAP.AddCurveView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   3base"clr-namespace:GAP.ExtendedControls     5   	      AddCurve  � Title#  �   5   	   $	��250��6,   $	��460��69   $	��400��6E   $	��250��6T     WindowDialog#��[ 5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5	       �� 
TargetType �� Property �� Value��%R   '��       �� 5
   
   " ��6   .�5      ��  � ��$ 23��6'   5      5      .��5      � $��10q�6    � ColumnDefinitions 5   
   �� 5        MaxWidthColumnDefinition#��[ 6   �� 5        AutoWidthColumnDefinition#��[ 6   �� 5        MinWidthColumnDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6    � RowDefinitions 5   
   �� 5        MaxHeightRowDefinition#��[ 6   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5       #��[ 6   �� 5!      #��[ 6   �� 5"      #��[ 6   �� 5#      #��[ 6   �� 5$      #��[ 6   .+�5&   
   ��   ChartThisCurve_TitleText#���  6   $��1��6F   $��1��6S   �� 5'   
   -   5         ��Name$ 	cmbCharts��5'      $��1��6'   $��3��64   	 �� DisplayMemberPath$
	 Name��6D   ��6]   �� *Charts+
 �� SelectedItem
 6|   �� *SelectedChart+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5)   
    	 TrackThisCurve_TitleText#��� 	 6   $��3��6F   $��1��6S   �� 5*   
   $��3��6   $
	 Name��6    $��3��69   ��6I   �� *Tracks+$ TwoWay=�$ PropertyChanged=�
 6�   �� *SelectedTrack+$ TwoWay=�$ PropertyChanged=��� 5,   
    
 RelatedProject#��� 
 6   $��5��6=   $��1��6J   �� 5-   
   $��5��6   $��3��6    $
	 Name��6Q   ��60   �� *
Projects+
 6j   �� *SelectedProject+$ TwoWay=�$ PropertyChanged=��� 5/   
   $��7��6   $��1��6!     RelatedWell#���  61   �� 50   
   $��7��6   $��3��6    $
	 Name��60   ��6I   �� *Wells+
 6g   �� *SelectedWell+$ TwoWay=�$ PropertyChanged=��� 52   
     RelatedDataset#���  6   $��9��6<   $��1��6I   �� 53   
   $��9��6   $��3��6    $
	 Name��60   ��6I   �� *
Datasets+
 6j   �� *SelectedDataset+$ TwoWay=�$ PropertyChanged=��� 55   
   $��11��6   $��3��6'     SaveCancelButton#��[ 67   56      57      