   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	   /clr-namespace:GAP.ConvertersGAP.Converters  5   	   A =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL 5   	        GAP.AddRemoveLithologyView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   M IGAP.CustomControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-extendedhttp://nearcoding.com/controls  5   	   -base"clr-namespace:GAP.ExtendedControls  5   	   -
convertersclr-namespace:GAP.Converters  5   	   .BLL$clr-namespace:GAP.BL;assembly=GAP.BL  5   	      WindowDialogResize#��[  5   9     � WindowStartupLocation$  CenterScreen=�5   	     Add/RemoveLithology � Title# �  5	   	   $	��500��67   $	��800��6D    J� InputBindings 5
      �� 5   
   
 �� Key$ A��6    �� 	Modifiers$ Controlf�6E    �� Command 6   �� *AddLithologyCommand+6E    � 	Resources 5      *   $GAP.Converters.LithologyImageBinding  LithologyImageBinding �� 
TargetType �� Property	 �� Value#  GAP.BL.TrackIDToNameConverter  TrackIDToNameConverter��%�   &	       '��      &	 _       5   
   6+   �� 5   
   " ��6   .�5      ��  � ��$	 Center=�62   5        5   
   6%   5      .��5      � $��10q�6   
 � ColumnDefinitions
 5   
   �� 5        AutoWidthColumnDefinition#��[ 6   �� 5        MinWidthColumnDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5        MaxWidthColumnDefinition#��[ 6    � RowDefinitions 5   
   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5       	 MaxHeightRowDefinition#��[	 6   �� 5       #��[ 6   �� 5!      #��[ 6   �� 5"      #��[ 6   �� 5#      #��[ 6   �� 5$      #��[ 6   �� 5%      #��[ 6   �� 5&      #��[ 6   �� 5'      #��[ 6   �� 5(      #��[ 6   �� 5)      #��[ 6   �� 5*      #��[ 6   �� 5+      #��[ 6   �� 5,      #��[ 6   �� 5-      #��[ 6   .+�5/   
   ��  	
 Chart#��� 
 6   �� 50   
   $
��Left=�6   $	��150��6.   $��2��6:    �� DisplayMemberPath$
 Name��6i   ��6J   �� *Charts+ �� SelectedItem 6�   �� *SelectedChart+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 52   
    	 Track#���  6   $��2��63   $	��Top=�6@   �� 53   
   $
��Left=�6    �� SelectionMode$ Extended=�6-     MetroListBox#��[ 6F   $	��150��6l   $��2��6x   $��2��6�   ��6�   �� *Tracks+ 6�   �� *SelectedTrack+��54      �� 55      .R�56      �� $��Center=�6=   $��0 0 5 0q�6X   ��6   �� *	TrackName+ d� 	IsChecked 6i   �� *IsTrackSelected+$ TwoWay=�$ PropertyChanged=�57      59      �� 5;   
     InitialDepth#���  6   $��4��6:   (  "GAP.CustomControls.ExtendedTextBox  5<   
   $
��Left=�6#   $��4��6>   $��2��6K   $	��150��6[   $��23��6g   ��5=      �� *CurrentObject.InitialDepth+$ TwoWay=�$ PropertyChanged=��� 5?   
     
FinalDepth#���  6   $��6��68     5@   
   -   5          Name$ ExtendedFinalDepth��5@   #   $
��Left=�6?   $��6��6Z   $��2��6g   $	��150��6w   $��23��6�   ��5A      �� *CurrentObject.FinalDepth+$ TwoWay=�$ PropertyChanged=��� 5C   
     SelectImage#���  6   $��8��69   � 5F   
   $��8��6   $��2��6   
 5G      �� 5H      #��[ 6#   �� 5I      #��[ 6#   �� 5J      #��[ 6#   .+�5M      �� $��30��6   $��20��6   $
��Left=�6�   ��6+   �� *FullImageName+$ TwoWay=�$ PropertyChanged=� �� 	Converter# [ 6�   �� 5N      #���  6   $��2��6=   $��Right=�6N     MetroButton#��[ 6j   ��6�   �� *SelectImageCommand+5O      �� 5Q   
   #��� ��6   $��10��62   6B   �� 5R   
   $	��150��6   $	��150��6   $��10��6�   $��2��6�   ��6-   �� *CurrentObject.LithologyName+$ TwoWay=�$ PropertyChanged=�6�   �� 5T   
   $��2��6   #��[ 6!   $	��150��6F   $��Right=�6R   $��12��6n     	AddToList#���  6|   ��6�   �� *AddLithologyCommand+6�   )  #GAP.CustomControls.ExtendedDataGrid  5V   
   $��4��6$   $��Stretch=�64   $��0��6R   $��14��6_   $��Stretch=�6q     MetroDataGrid#��[ 5W      ��65   �� *LithologiesToShow+&   System.Windows.Controls.DataGrid   Columns 5X      4  .System.Windows.Controls.DataGridTemplateColumn  5Y         CellTemplate 5Z      �� 5[      .R�5\      �� $��Center=�6'    6D   �� *IsLithologySelected+$ TwoWay=�$ PropertyChanged=�5]      ,  &System.Windows.Controls.DataGridColumn   Header 5_      �� 5`       6#   ��  �� RelativeSource ��  �� AncestorType$ WindowL� �� Path$$ DataContext.AllRecordsSelected�$ TwoWay=�$ PropertyChanged=�5b      0  *System.Windows.Controls.DataGridTextColumn  5c      # �  6%   1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 6M   �� *InitialDepth+  5d      # �  6%    6K   �� *
FinalDepth+  5e      # �  6%      Width5
  /System.Windows.Controls.DataGridLengthConverter$ *
 6�    6F   �� *
RefTrack+# [ 6�     5f      # � ��6%   $ *
 6g    6E   �� *LithologyName+6g     5g       	 Image# �  6)    5h      �� 5i      .R�5j      �� $��30��6$   $��30��6/   ��6;   �� *	ImageFile+# [ 5k      5m      5p      � 5q   
   $��16��6   $��6��6   
 5r      �� 5s      #��[ 6#   �� 5t      #��[ 6#   �� 5u      #��[ 6#   �� 5v      #��[ 6#   �� 5w      #��[ 6#   .+�5y      ��   DeleteRecords#���  6   #��[ 6t   ��6?   �� *" DeleteSelectedLithologiesCommand+6t   �� 5z      $��2��6   $��2��6-     SaveCancelButton#��[ 6A   5{      5|      5}      