   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I EMahApps.Metro, Version=0.13.1.0, Culture=neutral, PublicKeyToken=null   HorizontalScrollBar  �� 
TargetType �Name � 	Resources  Key'   !System.Windows.VisualStateManager    VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime	 O� Value
 �� Stretch  
GrayBrush1 F�Name � UseLayoutRounding J� Opacity  
BlackBrush J� Effect-  'System.Windows.Media.Effects.BlurEffect ��Name  
WhiteBrush �� RadiusY �� RadiusX  
ThumbBrush � ColumnDefinitions ��Name �� Interval  HorizontalDecrementTemplate ]�Name ]� DecreaseRepeatButton  RepeatButtonTemplate ]� Thumb  HorizontalThumbTemplate ]� IncreaseRepeatButton 	 HorizontalIncrementTemplate 
 VerticalScrollBar � RowDefinitions  VerticalDecrementTemplate  VerticalThumbTemplate  VerticalIncrementTemplate  MetroScrollBar �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value  MetroScrollViewer+  %MahApps.Metro.Controls.ControlsHelper  ��Name! ��Name" �� CanContentScroll# �� CanHorizontallyScroll$ �� CanVerticallyScroll% ��Name4  .System.Windows.Automation.AutomationProperties&   AutomationId' � Cursor( �� ScrollableHeight) � Maximum* � Minimum*+ �� #ComputedVerticalScrollBarVisibility, �� ViewportHeight- �� ViewportSize. � Value/ �� Mode0 �� RelativeSource1 �� ScrollableWidth2 �� Orientation,3 �� %ComputedHorizontalScrollBarVisibility4 �� ViewportWidth5 �� Triggers6 �� 
TargetName�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     ;controls,http://metro.mahapps.com/winfx/xaml/controls  1system&clr-namespace:System;assembly=mscorlib  %�  &	        &	
 6    &	 �    &	 	    �� "  ��.R�� $
 Root�� ���� RepeatButtonTemplate "  ��.R�� $
 Root��
������    CommonStates��.   
Normal���� HorizontalIncrementTemplate "  ��.R�� $
 Root��   CommonStates��.   
Normal��  	MouseOver��. �� .��U� $ (UIElement.Opacity)�$ 	HoverPath��.\�  $ 0:0:0.1��$	 1Q�U� $ (UIElement.Opacity)�$
 Blur��.\�  $ 0:0:0.1��$		 0.5Q�  Pressed��. ��   Disabled��. �� .��U� $ (UIElement.Opacity)�$
 Root��.\�  $ 0:0:0.1��$		 0.5Q�.+�F� +����X�u��X�u�YfѨ����X�u�$��10��$
 Uniform=�$��6��#���  F� $ 	HoverPath��+����X�u��X�u�YfѨ����X�u�$��10��$
 Uniform=�$��6�� .  $ 0Q�$��1,1,0,0q�#���  F� $
 Blur��+����X�u��X�u�YfѨ����X�u�$��10��$
 Uniform=�$��6�� .  $ 0Q�$��1,1,0,0q�#���     �� $ DisabledElement��#���  $ 0Q�$ 2��$ 2���� HorizontalDecrementTemplate "  ��.R�� $
 Root��   CommonStates��.   
Normal��  	MouseOver��. �� .��U� $ (UIElement.Opacity)�$ 	HoverPath��.\�  $ 0:0:0.1��$	 1Q�U� $ (UIElement.Opacity)�$
 Blur��.\�  $ 0:0:0.1��$		 0.5Q�  Pressed��. ��   Disabled��. �� .��U� $ (UIElement.Opacity)�$
 Root��.\�  $ 0:0:0.1��$		 0.5Q�.+�F� +������YfѠ���P�;��Ѡ��Yf$��10��$
 Uniform=�$��6��#���  F� $ 	HoverPath��+������YfѠ���P�;��Ѡ��Yf$��10��$
 Uniform=�$��6�� .  $ 0Q�$��0,1,0,0q�#���  F� $
 Blur��+������YfѠ���P�;��Ѡ��Yf$��10��$
 Uniform=�$��6�� .  $ 0Q�$��0,1,0,0q�#���     �� $ DisabledElement��#���  $ 0Q�$ 2��$ 2���� HorizontalThumbTemplate "  n�.R��    CommonStates��.   
Normal��  	MouseOver��. �� .��U� $ (UIElement.Opacity)�$ MouseOverRectangle��.\�  $ 0:0:0.1��$	 1Q�  Pressed��. �� .��U� $ (UIElement.Opacity)�$ PressedRectangle��.\�  $ 0:0:0.1��$	 1Q�  Disabled��. �� .��U� $ Opacity�$ ThumbVisual��.\��� $ 0:0:0��$	 0Q�.+�� $ ThumbVisual��$��0,1,0,1q�.+��� $ 
Background��$��1��#���  �� $ MouseOverRectangle��$��1��$ 0Q�#���  �� $ PressedRectangle��$��1��$ 0Q�#���     CommonStates��.   
Normal��  	MouseOver��  Disabled��. �� .��U� $ Opacity�$
 Root��.\��� $ 0��$		 0.5Q�.+�� $ HorizontalRoot��$��13�� �� $
��Auto ��� $��0.00001* ��� $
��Auto �.+��� $ HorizontalSmallDecrease��$��ScrollBar.LineLeftCommand��$��16����.  $ 50��$��1q�$��0��#���  ]� $ 
PART_Track����.  $��1�� �� $ HorizontalLargeDecrease��$��ScrollBar.PageLeftCommand����.  $ 50��#���   n� #���   �� $ HorizontalLargeIncrease��$ ��ScrollBar.PageRightCommand����.  $ 50��#���  �� $ HorizontalSmallIncrease��$ ��ScrollBar.LineRightCommand��$��16����.  $ 50��$��1q�$��2��#��� 	 �� "  ��.R�� $
 Root�� ���� RepeatButtonTemplate "  ��.R�� $
 Root��
������    CommonStates��.   
Normal���� VerticalIncrementTemplate "  ��.R�� $
 Root��   CommonStates��.   
Normal��  	MouseOver��. �� .��U� $ (UIElement.Opacity)�$ 	HoverPath��.\�  $ 0:0:0.1��$	 1Q�U� $ (UIElement.Opacity)�$
 Blur��.\�  $ 0:0:0.1��$		 0.5Q�  Pressed��. ��   Disabled��. �� .��U� $ (UIElement.Opacity)�$
 Root��.\�  $ 0:0:0.1��$		 0.5Q�.+�F� +������u0��2G �u0�П���Ѹ��u0$��6��$
 Uniform=�$��10��$��1,0,0,0q�#���  F� $ 	HoverPath��+������u0��2G �u0�П���Ѹ��u0$��6��$
 Uniform=�$��10�� .  $ 0Q�$��1,0,0,0q�#���  F� $
 Blur��+������u0��2G �u0�П���Ѹ��u0$��6��$
 Uniform=�$��10�� .  $ 0Q�$��1,0,0,0q�#���     �� $ DisabledElement��#���  $ 0Q�$ 2��$ 2���� VerticalDecrementTemplate "  ��.R�� $
 Root��   CommonStates��.   
Normal��  	MouseOver��. �� .��U� $ (UIElement.Opacity)�$ 	HoverPath��.\�  $ 0:0:0.1��$	 1Q�U� $ (UIElement.Opacity)�$
 Blur��.\�  $ 0:0:0.1��$		 0.5Q�  Pressed��. ��   Disabled��. �� .��U� $ (UIElement.Opacity)�$
 Root��.\�  $ 0:0:0.1��$		 0.5Q�.+�F� +�����2G �X
Ѹ��X
�П�б�	��2G �X
$��6��$
 Uniform=�$��10��$��	0.5,0,0,0q�#���  F� $ 	HoverPath��+�����2G �X
Ѹ��X
�П�б�	��2G �X
$��6��$
 Uniform=�$��10�� .  $ 0Q�$��1,0,0,0q�#���  F� $
 Blur��+�����2G �X
Ѹ��X
�П�б�	��2G �X
$��6��$
 Uniform=�$��10�� .  $ 0Q�$��1,0,0,0q�#���     �� $ DisabledElement��#���  $ 0Q�$ 2��$ 2���� VerticalThumbTemplate "  n�.R��    CommonStates��.   
Normal��  	MouseOver��. �� .��U� $ (UIElement.Opacity)�$ MouseOverRectangle��.\�  $ 0:0:0.1��$	 1Q�  Pressed��. �� .��U� $ (UIElement.Opacity)�$ PressedRectangle��.\�  $ 0:0:0.1��$	 1Q�  Disabled��. �� .��U� $ Opacity�$ ThumbVisual��.\��� $ 0:0:0��$	 0Q�.+�� $ ThumbVisual��$��1,0,1,0q�.+��� $ 
Background��$��1��#���  �� $ MouseOverRectangle��$��1��$ 0Q�#���  �� $ PressedRectangle��$��1��$ 0Q�#���     CommonStates��.   
Normal��  	MouseOver��  Disabled��. �� .��U� $ Opacity�$
 Root��.\��� $ 0��$		 0.5Q�.+�� $ VerticalRoot��$��13�� �� $
��Auto ��� $��0.00001* ��� $
��Auto �.+��� $ VerticalSmallDecrease��$��ScrollBar.LineUpCommand��$��16����.  $ 50��$��1q�$��0��#���  ]� $ 
PART_Track����. $��1�� ��X�(��       *31SystemParameters.VerticalScrollBarButtonHeightKey+)50Q� �� $ VerticalLargeDecrease��$��ScrollBar.PageUpCommand����.  $ 50��#���   n� #���   �� $ VerticalLargeIncrease��$��ScrollBar.PageDownCommand����.  $ 50��#���  �� $ VerticalSmallIncrease��$��ScrollBar.LineDownCommand��$��16����.  $ 50��$��1q�$��2��#���  �� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle .  P�  �@��Orientation$ 
Horizontal=�. ���  � ��$
 Auto����  � ��$ 14����  � ��# �   P�  �@��Orientation$ Vertical=�. ���  � ��$ 14����  � ��$
 Auto����  � ��# � 
 �� " ��.��� # �@ VerticalScrollBarOnLeftSide .  ��  � �� �� "  ��.R�� $
 Grid��#��z�� �� $  
leftColumn��$��* ��� $  rightColumn��$
��Auto � �� $��* ��� $
��Auto �.+��� $!! PART_ScrollContentPresenter��#" z��# .  $ .  #��z��#��z��$��0��#��z��$��0���� $% PART_VerticalScrollBar��$& VerticalScrollBar��$' Arrow��$��1��#) z( $* 0Q�$��0��#y�z+ #- z, . �� *VerticalOffset+$/ OneWay=�0 �� *TemplatedParent+�� $% PART_HorizontalScrollBar��$& HorizontalScrollBar��$' Arrow��$��0��#) z1 $* 0Q�$2 
Horizontal=�$��1��#y�z3 #- z4 . �� *HorizontalOffset+$/ OneWay=�0 �� *TemplatedParent+5 P� # �@ VerticalScrollBarOnLeftSide . . ��� $6 
leftColumn�� � ��$
 Auto ��� $6 rightColumn�� � ��$
 Auto ��� $!6 PART_ScrollContentPresenter�� � ��$ 1���� $6 PART_VerticalScrollBar�� � ��$ 0��