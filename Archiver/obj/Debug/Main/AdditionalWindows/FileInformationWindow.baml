   M S B A M L   `   `   `  ����C  ?Archiver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Archiver.Main.ConvertersArchiver.Main.Converters  5   	   Q-clr-namespace:Archiver.Main.AdditionalWindowsArchiver.Main.AdditionalWindows  5   	   ;    5Archiver.Main.AdditionalWindows.FileInformationWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7conv&clr-namespace:Archiver.Main.Converters     5   	   ?local-clr-namespace:Archiver.Main.AdditionalWindows     5   	     � Title$,  &Свойства файла/папки��5	   	   $	��400��6&   $	��500��63    � 	Resources 5
      =  7Archiver.Main.Converters.LabelInfoForFileOrDirConverter   	labelConv �� 
TargetType �� Property  ControlForeground �� Value��%m   &	        '��        5   
   6.   �� 5   
   " ��6   .�5      ��  � ��# �  6+   5      5      .��5      �   ControlLightBackground#���  6    � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       � ColumnDefinitions 5   
   �� 5      $��60 �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��60 �6   .+�5    
   �� $��1��6   $��1��6$   ��61   �� *	Element+ �� 	Converter# [  �� 5!   
   $��2��6   $��1��6$   ��61   �� *Element.Info.FullName+�� 5#   
   $��2��6   $��1��6!   $!��Время создания��61   �� 5$   
   $��2��6   $��2��6!   ��61   ��  �� StringFormat$ {0:g}��	 �� ConverterCulture$	 ru��
 �� Path$
 Element.Info.CreationTime��� 5&   
   $��3��6   $��1��6!   $2��,Последнее время доступа��61   �� 5'   
   $��3��6   $��2��6!   ��61   �� $ {0:g}��$	 ru��$!
 Element.Info.LastAccessTime��� 5)   
   $��4��6   $��1��6!   $��Размер��61   �� 5*   
   $��4��6   $��2��6!   ��61   �� *Size+ �� IsAsync . 5-      5.      