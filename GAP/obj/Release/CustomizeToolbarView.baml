   M S B A M L   `   `   `  ���� @  <GAP, Version=1.0.1.141, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls      GAP.CustomizeToolbarView   +base"clr-namespace:GAP.ExtendedControls  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml        CustomizeToolbar  � Title#  �     WindowDialogResize#��[  � WindowStartupLocation$ CenterScreen=� J� InputBindings �� 
 �� Key$ I�� �� 	Modifiers$ Controlf� �� Command �� *InsertSeparatorCommand+.��� $��5q� � ColumnDefinitions ��   MaxWidthColumnDefinition#��[ ��   MinWidthColumnDefinition#��[ ��   AutoWidthColumnDefinition#��[ �� #��[ �� #��[ �� #��[ �� #��[  � RowDefinitions ��   AutoHeightRowDefinition#��[ ��   MinHeightRowDefinition#��[ ��   MaxHeightRowDefinition#��[ �� #��[ �� #��[ .+��� $
��Bold"�  ItemsAvailable#���  $��Center=��� $
��Bold"� 	 ItemsInTheToolbox#��� 	 $��Center=�$��4���� $��2������ *AvailableToolbarItems+ �� SelectedItem �� *SelectedAvailableItem+���� .R��� 	 �� Orientation$	 
Horizontal=�.+��� $��60��$��40������ *Image+�� $��Center=����� *DisplayText+�� $��2��$��Center=�$��2��.+���  #
 RoundButtonWithAccentBackground#��[
 $��45��$��50������ *AddItemToToolboxCommand+���� $��../Images/MoveToRight.png���� #��[
 $��45��$��50��$��	0 10 0 10q����� *AddAllItemsToToolboxCommand+���� $"��../Images/MoveAllToRight.png���� #��[
 $��45��$��50��$��0 0 0 10q����� *RemoveItemFromToolboxCommand+���� $��../Images/MoveToLeft.png���� #��[
 $��45��$��50��$��0 0 0 10q����� *" RemoveAllItemsFromToolboxCommand+���� $!��../Images/MoveAllToLeft.png���� -   
 ��Name$
 ListBoxToolbox�� J� 	AllowDrop . $��4��$��2������ *ToolbarItems+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� *SelectedToolbarItem+���� .R��� $	 
Horizontal=�.+��� $��60��$��40������ *Image+�� $��Center=����� *DisplayText+�� $��Center=�$��6��$��2��$��0 10 0 0q�.+��� #��[
 $��45��$��50��$��0 0 0 10q� � ToolTip$ Up������ *	UpCommand+���� $��../Images/UpArrow.png���� #��[
 $��45��$��50��$��0 0 0 10q�$
 Down������ *DownCommand+���� $��../Images/DownArrow.png���� $	 
Horizontal=�$��4��$��0��$��7��$��0 0 10 0q�$��Right=�.+��� $	 
Horizontal=�$��0 10 0 0q�.+��� $��	0 0 20 10q�$	��150��$��35��  MetroButton#��[ $
��Left=�  InsertSeparatorButton#���  ���� *InsertSeparatorCommand+�� $��	0 0 20 10q�$	��160��$��35��#��[ $
��Left=�  RemoveSeparatorButton#���  ���� *RemoveSeparatorCommand+��   SaveCancelButton#��[ 