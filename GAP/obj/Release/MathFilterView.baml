   M S B A M L   `   `   `  ���� A  =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL  @ <GAP, Version=1.0.1.141, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls    GAP.MathFilterView   ,BLL$clr-namespace:GAP.BL;assembly=GAP.BL  +base"clr-namespace:GAP.ExtendedControls  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml        WindowDialog#��[    � WindowStartupLocation$  CenterScreen=�  
MathFilter � Title# �  $	��620��$	��390�� � 	Resources  �� MergedDictionaries �� Source �� 
TargetType �� Property �� Value��  �� $E ?pack://application:,,,/Style/BaseResource.CustomScrollbars.xaml?�%1   '��       �� " ��.���  � ��$ Center=�.��� $��5q� � RowDefinitions ��   AutoHeightRowDefinition#��[ ��   MinHeightRowDefinition#��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ 	 � ColumnDefinitions	 ��   AutoWidthColumnDefinition#��[ ��   MinWidthColumnDefinition#��[ �� #��[ .+���   Project#���  �� $
��Left=�$��2��
 �� DisplayMemberPath$

 Name�� �� SelectedValuePath$ ID�� �� SelectedItem �� *SelectedProject+���� *
Projects+�� $��2��  Well#���  �� $��2��$��2��$

 Name��$ ID�� �� *SelectedWell+���� *Wells+�� $��4��  Dataset#���  �� $��4��$��2��$

 Name��$ ID�� �� *SelectedDataset+���� *
Datasets+�� $��6�� 
	 Filter#��� 	 �� $��6��$��2������ *Filter+�� $��8�� 
 
NewDataset#��� 
 �� $��8��$��2������ *CurrentObject.Name+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� $��10��  SystemNotes#���  �� |�.   �� 
IsReadOnly .   TextBoxNotes#��[ $��10��$��2������ *CurrentObject.SystemNotes+$ TwoWay=�$ PropertyChanged=�.��   CustomScrollbar �� BasedOn��%g   '��       7  �� " ��8   .���  � ��$ 0 0 -3 0q���  � ��$ 0q���  � ��$ 10��.���� $��12��  	UserNotes#���  �� #��[ $��12��$��2������ *CurrentObject.UserNotes+$ TwoWay=�$ PropertyChanged=�.�� ��%g   '��       7  �� " ��8   .���  � ��$ 0 0 -3 0q���  � ��$ 0q���  � ��$ 10��.����   MetroGroupBox#��[ $	��360��$��14��$��0��$��3������   CurveProperties#���  $
��Bold"�.���� .+���   LineStyleProperties#��[ ��   MarkerStyleProperties#��[ ��   SaveCancelButton#��[ $��16��$��2��