   M S B A M L   `   `   `  ����C  ?Deadzone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5(clr-namespace:Deadzone;assembly=DeadzoneDeadzone  5      J FtoggleIndicator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:toggleIndicator;assembly=toggleIndicatortoggleIndicator 6H   J FliveXboxMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:liveXboxMonitor;assembly=liveXboxMonitorliveXboxMonitor 6�   L HliveXboxMonitorV2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:liveXboxMonitorV2;assembly=liveXboxMonitorV2liveXboxMonitorV2 6�    PilotPreferences%   PilotPreferences.ToolboxControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =Deadzone(clr-namespace:Deadzone;assembly=Deadzone     5      RtoggleIndicator6clr-namespace:toggleIndicator;assembly=toggleIndicator     6H   RliveXboxMonitor6clr-namespace:liveXboxMonitor;assembly=liveXboxMonitor     6�   XliveXboxMonitorV2:clr-namespace:liveXboxMonitorV2;assembly=liveXboxMonitorV2     6�   .��5	      (�   (� Stretch$
  Fill=�6   .��5
   
   � -   5         �Name$ mainGrid��5
       J� RenderTransformOrigin$ 0.501,0.443&�6!   ���	#FF333333$	��300��6\   $	��700��6i   .+�5      �� $��Pilot Preferences��6   $��Center=�60   $	��Top=�6M   ���	#FF1D1D1D���	#FFFF5607$
��Bold"�6�   $��20(�6�   $	��700��6�   $��Center=�6�   $��1q�6�   
���   �6�   (� 5      -   5         (�Name$ settingsItems��5      $��28,66,40,39q�6-   $
  Fill=�6B   .��5      � $	��195��6   $	��632��6$   .+�5      �� $��Center=�6   $	��142��69   $	��Top=�6F   $	��180��6^   $%��Resources/XBox360Controller.png��6j   $��232,0,220,0q�6�   �� 5      $��	Top Speed��6   $��Center=�60   $��277,142,265,0q�6M   $	��Top=�6d   ���	#FFFF5607$
��Bold"�6�   $��16(�6�   �� 5      -   5         ��Name$ speedSlider��5      $
��Left=�62   $��232,173,0,0q�6M   $	��Top=�6b   $	��136��6z    � Maximum$ 5Q�6�    �� TickFrequency$
 0.25Q�6�   �� 5      -   5         ��Name$ 
speedLabel��5      $��0%��60   $
��Left=�6=   $��368,166,0,0q�6X   $	��Top=�6m   
�������$��14(�6�   $��44��6�      Deadzone.ToolboxControl  5      -   5          Name$ deadzoneLeft��5   .   $
��Left=�6D   $��0,51,0,0q�6_   $	��Top=�6q     5      -   5        $ deadzoneRight��5   .   $
��Left=�6E   $��
487,12,0,0q�6`   $	��Top=�6t   �� 5      
���   �$
��Left=�6-   $��2��6H   $��
143,41,0,0q�6S   
���   �$	��Top=�6v   $	��120��6�   $ 0.5,0.5&�6�   z�5      V� 5      .~�5   "   �� �� 5   "   �� 5   "   	 �� Angle$	 -9.168Q�62   T� 5   "   5      5      �� 5      
���   �$
��Left=�6-   $��2��6H   $��104,112,0,0q�6S   
���   �$	��Top=�6w   $	��200��6�   $ 0.5,0.5&�6�   z�5       V� 5!      .~�5"   "   �� �� 5#   "   �� 5$   "   $	 -54.796Q�62   T� 5%   "   5&      5(      �� 5)      
���   �$
��Left=�6-   $��2��6H   $��
346,39,0,0q�6S   
���   �$	��Top=�6v   $	��148��6�   $ 0.5,0.5&�6�   z�5*      V� 5+      .~�5,   "   �� �� 5-   "   �� 5.   "   $	 -21.205Q�62   T� 5/   "   50      52      �� 53      
���   �$
��Left=�6-   $��2��6H   $��337,111,0,0q�6S   
���   �$	��Top=�6w   $	��162��6�   $ 0.5,0.5&�6�   z�54      V� 55      .~�56   "   �� �� 57   "   �� 58   "   $	 33.02Q�62   T� 59   "   5:      5<      5=      5>      �� 5@      -   5        
 ��Name$
 stateButton��5@      $��SETTINGS��6*   $
��Left=�6=   $	��Top=�6X   $	��163��6p   $��	10,44,0,0q�6|   $��43��6�   ���	#FF252525���	#FFFF7400$
��Bold"�6�   $��24(�6�   
���  ��6�   �� 5A      $��XBox Controller Assigned:��6   $
��Left=�68   $��440,261,0,0q�6S   $	��Top=�6h   $	��168��6�   ���	#FFFF5D00$
��Bold"�6�   $  toggleIndicator.ToolboxControl  5B      -   5          Name$&  xboxControllerConnectedIndicator��5B   -   $
��Left=�6W   $��624,254,0,0q�6r   $	��Top=�6�   $��36��6�   $��49��6�   (� 5C      -	   5        $ 	liveItems��5C      $��28,56,84,37q�6)   $
  Fill=�6>   .��5D      � $	��207��6   $	��588��6$   .+�5E      &   liveXboxMonitorV2.ToolboxControl  -
   5          Name$ liveXboxControllerMonitor��5E   7   $
��Left=�6Z   $��	158,6,0,0q�6u   $	��Top=�6�   $	��191��6�   $	��316��6�   �� 5F      
���   �$
��Left=�6-   $��2��6H   $��	84,76,0,0q�6S   
���   �$	��Top=�6u   $	��134��6�   $ 0.5,0.5&�6�   z�5G      V� 5H      .~�5I   "   �� �� 5J   "   �� 5K   "   $	 -19.641Q�62   T� 5L   "   5M      5O      �� 5P      
���   �$
��Left=�6-   $��2��6H   $��347,122,0,0q�6S   
���   �$	��Top=�6w   $	��148��6�   $ 0.5,0.5&�6�   z�5Q      V� 5R      .~�5S   "   �� �� 5T   "   �� 5U   "   $	 11.012Q�62   T� 5V   "   5W      5Y      �� 5Z      
���   �$
��Left=�6-   $��71��6H   
���   �$	��Top=�6c   $	��100��6{   $��	0,100,0,0q�6�   �� 5[      -   5        $ leftStickXval��5[      $��NA��63   $
��Left=�6@   $	��Top=�6[   ���	#FFFF6800$
��Bold"�6�   $��16(�6�   $��
34,100,0,0q�6�   �� 5\      -   5        $ leftStickYval��5\      $��NA��63   $
��Left=�6@   $	��Top=�6[   ���	#FFFF6800$
��Bold"�6�   $��16(�6�   $��
34,131,0,0q�6�   �� 5]      $	��X: ��6   $
��Left=�6*   $	��Top=�6E   ���	#FFFF6800$
��Bold"�6t   $��16(�6�   $��	0,100,0,0q�6�   �� 5^      $	��Y: ��6   $
��Left=�6*   $	��Top=�6E   ���	#FFFF6800$
��Bold"�6t   $��16(�6�   $��	0,131,0,0q�6�   �� 5a      
���   �$
��Left=�6-   $��71��6H   
���   �$	��Top=�6c   $	��100��6{   $��488,136,0,0q�6�   �� 5b      -   5        $ rightStickXval��5b      $��NA��64   $
��Left=�6A   $��522,136,0,0q�6\   $	��Top=�6q   ���	#FFFF6800$
��Bold"�6�   $��16(�6�   �� 5c      -   5        $ rightStickYval��5c      $��NA��64   $
��Left=�6A   $��522,167,0,0q�6\   $	��Top=�6q   ���	#FFFF6800$
��Bold"�6�   $��16(�6�   �� 5d      $	��X: ��6   $
��Left=�6*   $	��Top=�6E   ���	#FFFF6800$
��Bold"�6t   $��16(�6�   $��488,136,0,0q�6�   �� 5e      $	��Y: ��6   $
��Left=�6*   $��488,167,0,0q�6E   $	��Top=�6Z   ���	#FFFF6800$
��Bold"�6�   $��16(�6�   5g      5h      5j      5k      5l      