   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	   !    GAP.LithologyPropertiesView   5      +base"clr-namespace:GAP.ExtendedControls  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � WindowStartupLocation$  CenterScreen=�6@      LithologyProperties � Title# �   5   	   $��454.89��67   $��551.2��6G     WindowDialog#��[ 6U    J� InputBindings 5      �� 5   
   
 �� Key$ Escape��67    �� Command 6   �� *CancelCommand+6F   �� 5   
    �� 	Modifiers$ Controlf�6@   $ D��6T    6   �� *DeleteLithologyCommand+6^   �� 5	   
   $ Controlf�6=   $ A��6Q    6   �� *AddLithologyCommand+6[   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5        MaxHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5        AutoHeightRowDefinition#��[ 6    � ColumnDefinitions 5   
   �� 5        MaxWidthColumnDefinition#��[ 6   .+�5   
   �� ��5      �� 5      $
��Bold"�6     ExistingLithologies#���  6.   ��5      �� 5      $��Hidden=�6   $��0q�5      $��Stretch=�6P   $��Stretch=�6l    �� SelectedItem 5   S   �� *SelectedLithology+��5   '   �� *ImagesCollection+��5      �� 5      .R�5   "   � 
�������$	��517��6?   	 � Orientation$	 
Horizontal=�6K   $��5q�6d   5      5       5#      �� 5$   
   $��2��6   
 �� Orientation$
 
Horizontal=�6"   $��Right=�6;   .+�5%      ��   DeleteButton#���  6@     MetroButton#��[ 6j   ��6   �� *DeleteLithologyCommand+6j   �� 5&      $��10 0 0 0q�6    	 	AddButton#��� 	 6P   #��[ 6v   ��6(   �� *AddLithologyCommand+6v   �� 5'      $��10 0 0 0q�6    
 ButtonCancel#��[
 6(   5(      5)      5,      