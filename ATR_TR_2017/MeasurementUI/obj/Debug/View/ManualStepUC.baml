   M S B A M L   `   `   `  ����D  @DataUtils, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null8*clr-namespace:DataUtils;assembly=DataUtils	DataUtils  5       MeasurementUI    MeasurementUI.ManualStepUC   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @	dataUtils*clr-namespace:DataUtils;assembly=DataUtils     5      e aDevExpress.Xpf.DemoBase.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49aAdxdb6http://schemas.devexpress.com/winfx/2008/xaml/demobase  5      d `DevExpress.Xpf.Docking.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a@dxdo5http://schemas.devexpress.com/winfx/2008/xaml/docking  5	      a	 ]DevExpress.Xpf.Core.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors 	 5
      f
 bDevExpress.Xpf.Grid.v14.2.Core, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49aa ]DevExpress.Xpf.Grid.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxg2http://schemas.devexpress.com/winfx/2008/xaml/grid 
  5      Jdxet?http://schemas.devexpress.com/winfx/2008/xaml/editors/themekeys 	 5      <dxb2http://schemas.devexpress.com/winfx/2008/xaml/bars 	 5      Fdxlc;http://schemas.devexpress.com/winfx/2008/xaml/layoutcontrol 	 5      ;dx2http://schemas.devexpress.com/winfx/2008/xaml/core 	 5      >dxn4http://schemas.devexpress.com/winfx/2008/xaml/navbar 	 5      -   5        & 	  DevExpress.Xpf.Bars.BarNameScope    IsScopeOwner  .  � 	Resources 5      % 	 DevExpress.Xpf.Editors.TextEdit �� 
TargetType �� Property �� Value��%�  '        '��     �� 5   
   "  6   .�5      ��  � ��$ 0,3,5,3q�6'   69   �� 5       � ��$ 26��6'   64   �� 5       � ��$ 14(�6)   66   �� 5       � ��$ 微软雅黑)�6,   6;   �� 5       � ��$	 130��6&   64   5      �� 5   
   " ��6   .�5      ��  � ��$ 36��6&   63   �� 5       � ��$ 20,3,3,0q�6'   6:   �� 5       � ��$ 14(�6)   66   �� 5       � ��$ Right=�64   6D   5       5!      .��5"      �  � RowDefinitions 5#   
   �� 5$      $
��Auto �6   �� 5%      $��* �6    � ColumnDefinitions 5'   
   �� 5(      $
��Auto �6   �� 5)      $��* �6   .+�5+   
   �� $��0��6   $��2��6%   .+�5,      ��  �� Orientation$ 
Horizontal=�6   .+�5-      �� $��列名��6   $��5q�6%     5.      -   5        $��columnNameTxt��5.   B   $	��120��6   $��25��6+   $��5q�67   % 	 DevExpress.Xpf.Editors.BaseEdit   	EditValue 6W   �� 	 �� Path$	 ColumnInfo.ColumnName�5/      �� 50      $ 
Horizontal=�6   .+�51      �� $��类型��6   $��5q�6%   ) 	 #DevExpress.Xpf.Editors.ComboBoxEdit  52      -   5        $��typeComboBox��52   F   $	��120��6#   $��25��6/   $��5q�6;   + 	 %DevExpress.Xpf.Editors.LookUpEditBase
   ItemsSource
 6Z   �� *
Typelist+ 6|   �� $	 ColumnInfo.ColumnTypeStr�53      54      �� 55      $ Vertical=�6   .+�56      �� -   5        $
��Left=�56      $
��45,5q�6   $	��100��6�   $��25��6�   $��添加��6�   ��64   �� *	AddColumn+��6R   �� $	 
ColumnInfo�6�   �� 57      -   5        $��	btnDelete��57   4   $
��Left=�6   $	��100��6E   $��25��6Q   $��删除��6]   $
��45,5q�6j   6x   58      59      �� 5:   
   $��0��6   $��2��6'   �� 5;   
   -   5        $��panelFileData��5;      $��0��6*   $��1��67   $ 
Horizontal=�6G   .+�5<      �� $��	起始点��6   $��46��6"     5=      -   5        $��txtStart��5=       6,   �� *	StartData+ �� 	Converter !   DataUtils.SIPrefixConverter  �� 5>      $��	截止点��6   $��46��6"     5?      -   5        $��txtStop��5?       6*   �� *
StopData+   �� 5@      $��点数��6     5A      -	   5        $��txtPoint��5A       6+   �� *Points+5B      %  DevExpress.Xpf.Grid.GridControl  5C   
   -
   5        $��manualgridControl��5C   g   $��1��6   $��1��6*   ) 
 #DevExpress.Xpf.Grid.DataControlBase   SelectionMode$	 Row=�67      AutoGenerateColumns$
 None=�6K      ItemsSource 6�   �� $	 ColumnTable� � ContextMenu 5D      �� 5E      .@�5F      m� -   5        $��	删除行��5F      6,   m� 5G      -   5        $��快速填充��5G      6-   5H         View 5J      #	  DevExpress.Xpf.Grid.TableView	  5K      -   5        $��manualgridView��5K   w    	  AllowPerPixelScrolling . &
 
  DevExpress.Xpf.Grid.DataViewBase  
  AllowMoveColumnToDropArea .  
  AllowSorting .   	  NewItemRowPosition$ Bottom=�6�    
  ShowTotalSummary .  &   DevExpress.Xpf.Grid.GridViewBase   ShowGroupPanel .  6�   5M      5N      5O      