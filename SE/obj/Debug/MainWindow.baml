   M S B A M L   `   `   `  ����=  9SE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:SESE  5   	       SE.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   "localclr-namespace:SE     5   	   -   5          �Name$  window��5   	    � Title$ 
MainWindow��5   	    � WindowStartupLocation$ CenterScreen=�6   $	��600��6A    � 
ResizeMode$ NoResize=�6M    � 	Resources 5	         TabControlNormalBorderBrush �� Color  TabControlStyle1 �� 
TargetType �� Property �� Value	 �� 
TargetType
 J� SnapsToDevicePixels-  'System.Windows.Input.KeyboardNavigation   TabNavigation � ColumnDefinitions ��Name � RowDefinitions ��Name ��Name K� IsItemsHost   TabIndex K� ZIndex ��Name   DirectionalNavigation ��Name �� Triggers P� Property P� Value �� 
TargetName��%;  &	        &	 !     7   �� 5
   
   $ #8C8E94��6>   �� 5   
   " ��6)   .�5      ��  � ��# � ��6+   �� 5       � ��$ 4,4,4,4q�6(   �� 5       � ��$ 1q�60   �� 5       � ��8   6,   �� 5       � �� �#F9F9F96+   �� 5       � ��$ Center=�6;   �� 5       � ��$ Center=�69   �� 5       � �� 5      �� 5      "	 ��6&   .R�5      � }�. 
 . $ Local=�6N    5      �� 5   "   $ ColumnDefinition0��63   �� 5   "   $ ColumnDefinition1��63   $��0 �6N    5      �� 5   "   $ RowDefinition0��60   $
��Auto �6H   �� 5   "   $ RowDefinition1��60   $��* �6H   .+�5      �� 
 . $
��Auto=�6F   $
��Auto=�6k   .��5    "   �� $ HeaderPanel��6+   $��0��6@    . $��2,2,2,0q�6c   $��0��6t   $ 1��6�   $ 1��6�   5!      �� 5"      $ ContentPanel��6%   #��z��6;   #��z��6g   #��z��6�   $��0��6�   $ 	Contained=�6�   $��1��6
  $ 2��6  $ Local=�67  ._�5#   "   �� $ PART_SelectedContentHost��63   $��SelectedContent��6U   #��z��6u   #
 z
 6�   5$      5%       5&      P� 5'       �@��TabStripPlacement$ Bottom=�6C   . �5(   "   �� $ HeaderPanel��6=    � ��$ 1��6V   �� 5)   "   $ ContentPanel��6=    � ��$ 0��6W   �� 5*   "   $ RowDefinition0��6;    � ��$ * �6W   �� 5+   "   $ RowDefinition1��6;    � ��$
 Auto �6W   �� 5,   "   $ HeaderPanel��6;    � ��$ 2,0,2,2q�6T   5-      P� 5.       �@��TabStripPlacement$
 Left=�6C   . �5/   "   �� $ HeaderPanel��6=    � ��$ 0��6V   �� 50   "   $ ContentPanel��6=    � ��$ 0��6W   �� 51   "   $ HeaderPanel��6@    � ��$ 0��6Y   �� 52   "   $ ContentPanel��6@    � ��$ 1��6Z   �� 53   "   $ ColumnDefinition0��6:    � ��$
 Auto �6Y   �� 54   "   $ ColumnDefinition1��6:    � ��$ * �6Y   �� 55   "   $ RowDefinition0��6;    � ��$ * �6W   �� 56   "   $ RowDefinition1��6;    � ��$ 0 �6W   �� 57   "   $ HeaderPanel��6;    � ��$ 2,2,0,2q�6T   58      P� 59       �@��TabStripPlacement$ Right=�6C   . �5:   "   �� $ HeaderPanel��6=    � ��$ 0��6V   �� 5;   "   $ ContentPanel��6=    � ��$ 0��6W   �� 5<   "   $ HeaderPanel��6@    � ��$ 1��6Y   �� 5=   "   $ ContentPanel��6@    � ��$ 0��6Z   �� 5>   "   $ ColumnDefinition0��6:    � ��$ * �6Y   �� 5?   "   $ ColumnDefinition1��6:    � ��$
 Auto �6Y   �� 5@   "   $ RowDefinition0��6;    � ��$ * �6W   �� 5A   "   $ RowDefinition1��6;    � ��$ 0 �6W   �� 5B   "   $ HeaderPanel��6;    � ��$ 0,2,2,2q�6T   5C      P� 5D       � {� .  . �5E   "   �� # � ��6?    � ��5F      5H      5J      5K      5L      .��5M      �  5N   
   �� 5O      �� 5P      $��0* �6   �� 5Q      $��0* �6   .+�5S   
   �� -   5         ��Name$ label��5S      $��请输入部件名称：��6   $
��Left=�62   $��	97,11,0,0q�6M   $	��Top=�6`   $��26��6x   $	��106��6�   �� 5T   
   -   5         ��Name$ 
schTextBox��5T      $
��Left=�6&   $��25��6A   $��
207,11,0,0q�6M    �� TextWrapping$
 Wrap=�6a   $	��Top=�6v   $	��254��6�   6�   �� 5U   
   -   5         ��Name$ 
tabControl��5U      $��
2,66,-2,11q�6)   $��3��6=   |�.   �� SelectedIndex$ 0��6�     �� TabStripPlacement$
  Left=�6�   #���  6�   .@�5V      �� -   5        ! ��Name$! 
schTabItem��5V      $��搜索��6*   $��0q�66   $��19��6A   $	��Top=�6M   $y�	Collapsed=�6e   .��5W      �� -   5        " ��Name$" fileListBox��5W      $��0q�6/   5X      �� 5Y      -   5        $! totalTablItem��5Y      $��全部��6-   $��64��69   $��0q�6D   $��Visible=�6O   $��19��6�   $��Bottom=�6�   .��5Z      �� -   5        $" totalListBox��5Z      $
��Auto=�60   $
��Auto=�6b   # J� 	AllowDrop# . |�.  6�   5[      5\      �� 5]   
   -	   5        $ ��Name$$ 
loadButton��5]      $
��Left=�6%   $��3,9,0,0q�6@   $	��Top=�6Q   $��64��6i   $��27��6�   $��加载��6�   �� 5^   
   -
   5        % ��Name$% checkAllBox��5^      $��全选��6(   $
��Left=�65   $��	81,45,0,0q�6P   $	��Top=�6c   6�   �� 5_   
   -   5        $$ rmAllButton��5_      $��批量删除��6&   $
��Left=�65   $��
128,42,0,0q�6P   $	��Top=�6d   $��75��6|   6�   5`      5a      