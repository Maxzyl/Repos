   M S B A M L   `   `   `  ����K  GViewModelBaseLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM8clr-namespace:ViewModelBaseLib;assembly=ViewModelBaseLibViewModelBaseLib  5
   	    MeasurementUI$   MeasurementUI.SelectConfigPath   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   a ]DevExpress.Xpf.Grid.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49aGdxgt<http://schemas.devexpress.com/winfx/2008/xaml/grid/themekeys  5   	   a ]DevExpress.Xpf.Core.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a;dx2http://schemas.devexpress.com/winfx/2008/xaml/core  5   	   <dxb2http://schemas.devexpress.com/winfx/2008/xaml/bars  5   	   d	 `DevExpress.Xpf.Docking.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a@dxdo5http://schemas.devexpress.com/winfx/2008/xaml/docking 	 5   	   f
 bDevExpress.Xpf.Grid.v14.2.Core, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxg2http://schemas.devexpress.com/winfx/2008/xaml/grid 
  5   	   ?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  5	   	   Avm8clr-namespace:ViewModelBaseLib;assembly=ViewModelBaseLib  5
   	   $	��300��5   	   $	��300��6     � WindowStartupLocation$  CenterScreen=�6#    � WindowStyle$ 
ToolWindow=�6H    � 
ResizeMode$ NoResize=�6a   .��5      � -   5        $
��grid��5      .+�5   
   �� -   5        $��listBox��5      ��6!   �� *ConfigPathList+��5      �� 5       �� 
TargetType" ��6"   .R�5      �� $
��Auto=�6#   $��Disabled=�6F   .��5      �  � Orientation$ 
Horizontal=�6$    K� IsItemsHost . 6=   5      5      ��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      ��  d� 	IsChecked 6#   �� *
IsSelected+�� 5      ��6$   �� *	PathStr+5      5      5      5      5      