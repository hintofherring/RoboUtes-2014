   M S B A M L   `   `   `  ����J  FtoggleIndicator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:toggleIndicator;assembly=toggleIndicatortoggleIndicator  5       WheelMonitor!   WheelMonitor.ToolboxControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      RtoggleIndicator6clr-namespace:toggleIndicator;assembly=toggleIndicator     5      -   5        $	��193��5      $	��219��6,   .��5      (� -   5          (�Name$  primaryViewBox��5      $	��193��6&    (� Stretch$
 Fill=�63   $	��Top=�6B   .��5	   
   � $	��193��6   $	��219��6   .+�5
      �� ���	#FF333333
���   �$
��Left=�68   $	��219��6S   $��	0,0,0,-46q�6_   $	��192��6r   $	��Top=�6   �� 5      $��Speed:��6   $
��Left=�6%   $��	77,34,0,0q�6@   $	��Top=�6S   
�������$
��Bold"�6~   $��14(�6�   �� 5      -   5         ��Name$ speed��5      $��0 m/s��6#   $
��Left=�63   $��
131,34,0,0q�6N   $	��Top=�6b   
�������$��14(�6�   $
��Bold"�6�   �� 5      -   5        $ title��5      $��Wheel��6#   $
��Left=�63   $��29��6N   $	��219��6Z   ���	#FF1D1D1D���	#FFFF5607$��16(�6�   $
��Bold"�6�   $	��Top=�6�   $��Center=�6�   �� 5      $��Current:��6   $
��Left=�6'   $��	67,63,0,0q�6B   $	��Top=�6U   
�������$
��Bold"�6�   $��14(�6�   �� 5      $��Stall:��6   $
��Left=�6%   $��	88,97,0,0q�6@   $	��Top=�6S   
�������$
��Bold"�6~   $��14(�6�   �� 5      -   5        $ current��5      $
��0 mA��6%   $
��Left=�64   $��
131,63,0,0q�6O   $	��Top=�6c   
�������$��14(�6�   $
��Bold"�6�   �� 5      $��Slip:��6   $
��Left=�6$   $��88,140,0,-23q�6?   $	��Top=�6U   
�������$
��Bold"�6�   $��14(�6�   (� 5      -   5        $  tireViewBox��5      $��33,45,118,10q�6+   $
 Fill=�6A   .��5      � $	��138��6   $��68��6$   .+�5      �� -   5         ��Name$ spinningOverlay��5       $
��Left=�69   $	��129��6T   ���	#00000000$	��Top=�6t   $��25��6�    K� ZIndex$ 1��6�    �� RadiusX$ 5��6�    �� RadiusY$ 10��6�   �� 5      $
��Left=�6   $	��137��67   $��0,1,0,0q�6D   $	��Top=�6U   $��68��6m   $��Resources/tire.png��6x   5      5      $   toggleIndicator.ToolboxControl  5      -   5          Name$ stallIndicator��5   -   $
��Left=�6E   $��
152,92,0,0q�6`   $	��Top=�6t   $��41��6�   $��41��6�     5      -	   5        $ slipIndicator��5   -   $
��Left=�6D   $��152,138,0,0q�6_   $	��Top=�6t   $��41��6�   $��41��6�   5      5      5      