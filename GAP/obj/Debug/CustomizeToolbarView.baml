   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	       GAP.CustomizeToolbarView   5      +base"clr-namespace:GAP.ExtendedControls  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	      CustomizeToolbar  � Title#  �   5   	     WindowDialogResize#��[ 65    � WindowStartupLocation$ CenterScreen=�6a    J� InputBindings 5      �� 5   
   
 �� Key$ I��6    �� 	Modifiers$ Controlf�6    �� Command 61   �� *InsertSeparatorCommand+6^   .��5
      � $��5q�6    � ColumnDefinitions 5   
   �� 5        MaxWidthColumnDefinition#��[ 6   �� 5        MinWidthColumnDefinition#��[ 6   �� 5        AutoWidthColumnDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6    � RowDefinitions 5   
   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5        MaxHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   .+�5   
   �� $
��Bold"�6     ItemsAvailable#���  6&   $��Center=�6N   6m   �� 5   
   $
��Bold"�6    	 ItemsInTheToolbox#��� 	 6&   $��Center=�6Q   $��4��6n   6�   �� 5   
   $��2��6@   ��6   �� *AvailableToolbarItems+ �� SelectedItem 6M   �� *SelectedAvailableItem+��5       �� 5!      .R�5"      �� 	 �� Orientation$	 
Horizontal=�6!   .+�5#      �� $��60��6    $��40��6+   ��67   �� *Image+�� 5$      $��Center=�6$   ��6?   �� *DisplayText+5%      5&      5(      �� 5*   
   $��2��6   $��Center=�6%   $��2��6@   .+�5+      ��  #
 RoundButtonWithAccentBackground#��[
 6   $��45��6N   $��50��6Z   ��6f   �� *AddItemToToolboxCommand+��5,      �� 5-      $��../Images/MoveToRight.png��6   5/      �� 50      #��[
 6   $��45��6~   $��50��6�   $��	0 10 0 10q�6�   ��6N   �� *AddAllItemsToToolboxCommand+��51      �� 52      $"��../Images/MoveAllToRight.png��6   54      �� 55      #��[
 6   $��45��6N   $��50��6Z   $��0 0 0 10q�6�   ��6f   �� *RemoveItemFromToolboxCommand+��56      �� 57      $��../Images/MoveToLeft.png��6   59      �� 5:      #��[
 6   $��45��6O   $��50��6[   $��0 0 0 10q�6�   ��6g   �� *" RemoveAllItemsFromToolboxCommand+��5;      �� 5<      $!��../Images/MoveAllToLeft.png��6   5>      5?      �� 5A   
   -   5        
 ��Name$
 ListBoxToolbox��5A       J� 	AllowDrop . $��4��5B   >   $��2��6N   ��5A   <   �� *ToolbarItems+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� 5B      �� *SelectedToolbarItem+��5C      �� 5D      .R�5E      �� $	 
Horizontal=�6!   .+�5F      �� $��60��6    $��40��6+   ��67   �� *Image+�� 5G      $��Center=�6$   ��6?   �� *DisplayText+5H      5I      5K      �� 5L   
   $��Center=�6   $��6��60   $��2��6@   $��0 10 0 0q�6M   .+�5M      �� #��[
 6   $��45��6N   $��50��6Z   $��0 0 0 10q�6e    � ToolTip$ Up��6x   ��6�   �� *	UpCommand+��5N      �� 5O      $��../Images/UpArrow.png��6   5Q      �� 5R      #��[
 6   $��45��6N   $��50��6Z   $��0 0 0 10q�6e   $
 Down��6x   ��6�   �� *DownCommand+��5S      �� 5T      $��../Images/DownArrow.png��6   5V      5W      �� 5]   
   $	 
Horizontal=�6   $��4��6/   $��0��6<   $��7��6L   $��0 0 10 0q�6`   $��Right=�6r   .+�5^      �� $	 
Horizontal=�6   $��0 10 0 0q�62   .+�5_      �� $��	0 0 20 10q�6   $	��150��6,   $��35��68     MetroButton#��[ 6D   $
��Left=�6i     InsertSeparatorButton#���  6�   ��6�   �� *InsertSeparatorCommand+�� 5`      $��	0 0 20 10q�6   $	��160��6,   $��35��68   #��[ 6D   $
��Left=�6i     RemoveSeparatorButton#���  6�   ��6�   �� *RemoveSeparatorCommand+5a      �� 5b        SaveCancelButton#��[ 6   5d      5e      5f      