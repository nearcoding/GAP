   M S B A M L   `   `   `  ����I  EMahApps.Metro, Version=0.13.1.0, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType   MetroTabControl �� Property �� Value �� 
TargetType � ColumnDefinitions ��Name � RowDefinitions ��Name  AddChart � ToolTip  ButtonWithNoStyle	 �� ImageSource
 ��Name K� IsItemsHost-   'System.Windows.Input.KeyboardNavigation    TabIndex K� ZIndex ��Name  AccentColorBrush    DirectionalNavigation    TabNavigation ��Name J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName  MetroTabItem  
BlackBrush+   %MahApps.Metro.Controls.ControlsHelper   HeaderFontSize   HeaderFontStretch   HeaderFontWeight  GrayNormalBrush P� 
SourceName  GrayHoverBrush�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5      %R  &	        7  &	 �    �� 5      "  ��6   .�5   
   ��  � �� 5      Z� �� 5   
    � �� 5	      Z� �� 5
   
    � �� 5      �� 5      " ��6"   .R�5      �  5      �� 5      $ ColumnDefinition0��6/   $
��Auto �6J   �� 5      $ ColumnDefinition1��6/   $��* �6J    5      �� 5      $ RowDefinition0��6,   $
��Auto �6D   �� 5      $ RowDefinition1��6,   $��* �6D   .+�5      �� $��1��6"   $��0��62   $��55��6?   $
��Left=�6J   ��5      �� 5   "   # �  6)   |�.  8��  $��Center=�6�   $��30��6�   $��30��6�   ��6�   �� *AddTabCommand+��5   &   �� 5   *   $	 ../Images/AddChart.png��65   5   #   5      �� 5      $
 HeaderPanel��6#   $��0��5    #    . $��0��5"   #   $ 1��5#   #   $ 1��5$   #   �� 5%      $ ContentPanel��6!   #���  5&   !   $	��0.5q�5'   !   #��z��5(   !   $��0��5)   !   $��2��61   $ 	Contained=�5*   !   $��1��5+   !   $ 2��5,   !   $ Local=�5-   !   ._�5.      �� $ PART_SelectedContentHost��6/   $��SelectedContent��5/   /   #��z��50   /   # z 51   /   52      53       54      P� 55       �@��TabStripPlacement$ Bottom=�56   "   . �57      �� $ HeaderPanel��58   %    � ��$ 1��59   %   �� 5:      $ ContentPanel��5;   %    � ��$ 0��5<   %   �� 5=      $ RowDefinition0��5>   %    � ��$ * �5?   %   �� 5@      $ RowDefinition1��5A   %    � ��$
 Auto �5B   %   �� 5C      $ HeaderPanel��5D   %    � ��$ 2,0,2,2q�5E   %   5F      P� 5G       �@��TabStripPlacement$
 Left=�5H   "   . �5I      �� $ HeaderPanel��5J   %    � ��$ 0��5K   %   �� 5L      $ ContentPanel��5M   %    � ��$ 0��5N   %   �� 5O      $ HeaderPanel��5P   %    � ��$ 0��5Q   %   �� 5R      $ ContentPanel��5S   %    � ��$ 1��5T   %   �� 5U      $ ColumnDefinition0��5V   %    � ��$
 Auto �5W   %   �� 5X      $ ColumnDefinition1��5Y   %    � ��$ * �5Z   %   �� 5[      $ RowDefinition0��5\   %    � ��$ * �5]   %   �� 5^      $ RowDefinition1��5_   %    � ��$ 0 �5`   %   �� 5a      $ HeaderPanel��5b   %    � ��$ 2,2,0,2q�5c   %   5d      P� 5e       �@��TabStripPlacement$ Right=�5f   "   . �5g      �� $ HeaderPanel��5h   %    � ��$ 0��5i   %   �� 5j      $ ContentPanel��5k   %    � ��$ 0��5l   %   �� 5m      $ HeaderPanel��5n   %    � ��$ 1��5o   %   �� 5p      $ ContentPanel��5q   %    � ��$ 0��5r   %   �� 5s      $ ColumnDefinition0��5t   %    � ��$ * �5u   %   �� 5v      $ ColumnDefinition1��5w   %    � ��$
 Auto �5x   %   �� 5y      $ RowDefinition0��5z   %    � ��$ * �5{   %   �� 5|      $ RowDefinition1��5}   %    � ��$ 0 �5~   %   �� 5      $ HeaderPanel��5�   %    � ��$ 0,2,2,2q�5�   %   5�      5�      5�      5�      �� 5�      "  ��6   .�5�   
   ��  �@�FocusVisualStyle 5�      Z� �� 5�   
    � �� .  5�      �� 5�   
    � ��$ 6,2,6,2q�5�      �� 5�   
    � ��$ Stretch=�5�      �� 5�   
    � ��$ Stretch=�5�      �� 5�   
    � ��$ 5��5�      �� 5�   
    � ��$ 5��5�      �� 5�   
    � ��$ 0q�5�      �� 5�   
    � ��# �  5�      �� 5�   
    � ��
 ���� 5�      �� 5�   
    � ��# �  5�      �� 5�   
    �@J�SnapsToDevicePixels . 5�      �� 5�   
    �@ HeaderFontSize$ 20Q�5�      �� 5�   
    � �� 5�      �� 5�      " ��6"   .R�5�      �� $ Border��6   #��z��5�      #��z��5�      #��z��5�      #��z��5�      # z 5�      ._�5�      �� $ ContentSite��6+   $��2,1,2,1q�5�   +   #��z��5�   +   #��z��5�   +   #��z 5�   +   #��z 5�   +   #��z 5�   +   #��z��5�   +   $��Header��5�   +   ��. 5�   +   5�       5�      P� 5�       �@��
IsSelected . . �5�      �� $ ContentSite��5�   %   # �  5�   %    � ��5�      P� 5�       �@��
IsSelected .  . �5�      �� $ ContentSite��5�   %   # �  5�   %    � ��5�      P� 5�      $ ContentSite��6"    �@J�IsMouseOver . . �5�      �� $ ContentSite��5�   %   # �  5�   %    � ��5�      5�      5�      5�      5�      