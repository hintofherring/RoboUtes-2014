   M S B A M L   `   `   `  ����  ArmSideView    ArmSideView.ArmSide   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      $��286.667��5      $	��344��60   .��5      � .+�5   
   (�   (� Stretch$
  Fill=�6   .��5	      �� -   5         ��Name$
 canv��5	      ���	#FF0E0E0E$	��287��6:   $	��344��6G   .+�5
      �� $��GOAL SHOULDER Θ:��6   
 �� Top$	 254��63   $��33��6D   $	��122��6P   ���	#FF7E7E7E6\   �� 5      -   5         ��Name$ shoulderAngleLabel��5      $��NA��64    �� Bottom$ 0��6A   $��33��6S   $��44��6_   ���	#FF7E7E7E �� Left$	 122��6�   �� 5      $��GOAL ELBOW Θ:��6   $	 254��60   $��33��6A   $	��105��6M   ���	#FF7E7E7E$	 195��6p   �� 5      -   5        $ elbowAngleLabel��5      $��NA��61   $ 0��6>   $��33��6P   $��44��6\   ���	#FF7E7E7E$	 300��6~   �� 5      -   5        $ realElbowLabel��5      $��REAL ELBOW Θ:��60   $��33��6H   $	��120��6T   ���	#FF7E7E7E$	 180��6w   $	 180��6�   �� 5      -   5        $ realElbowAngleLabel��5      $��NA��65   $��33��6B   $��44��6N   ���	#FF7E7E7E$	 300��6p   $	 180��6�   �� 5      -   5        $ realShoulderLabel��5      $��REAL SHOULDER Θ:��63   $��33��6N   $	��120��6Z   ���	#FF7E7E7E$	 180��6}   $	 147��6�   �� 5      -   5        $ realShoulderAngleLabel��5      $��NA��68   $��33��6E   $��44��6Q   ���	#FF7E7E7E$	 300��6s   $	 147��6�   �� 5      ���	#FF666666$��27��6-   
���   �$	 222��6H   $��60��6Y   �� 5      ���	#FF666666$��17��6-   $ 24��69   
���   �$	 206��6Y   $��36��6j   �� 5      -   5         ��Name$ gRec1��5      $��20��6+   $ 38��67   
��� � �$ 80��6V   $	��142��6i    J� RenderTransformOrigin$ 0,0.5&�6u   $��3��6�    �� RadiusX$ 10��6�   	 �� RadiusY$	 10��6�   ��5      �� 5      
 �� Color$

 Lime��6*    �� Opacity$	 0.3Q�67   5      �� 5      -	   5        $ gRec2��5      $��20��6+   $	 180��67   $ 80��6I   $	��105��6\   $ 0,0.5&�6h   
��� � �$��3��6�   $ 10��6�   $	 10��6�   ��5      �� 5      $

 Lime��6*   $	 0.3Q�67   5      �� 5      -
   5        $ aRec1��5      $��20��6+   $ 38��67   
���  ��$ 80��6U   $	��142��6h   $ 0,0.5&�6t   $��3��6�   $ 10��6�   $	 10��6�   ��5      �� 5       $	
 Red��6*   $	 0.3Q�66   5"      �� 5#      -   5        $ aRec2��5#      $��20��6+   $	 180��67   $ 80��6I   $	��105��6\   $ 0,0.5&�6h   
���  ��$��3��6�   $ 10��6�   $	 10��6�   ��5$      �� 5%      $	
 Red��6*   $	 0.3Q�66   5'      �� 5(      $��72��6   $ 10��6%   $ 10��66   $��56��6F   .+�5)      �� $��46��6   $ 14��6)   $��18��6:   .+�5*      F� -   5         F�Name$ VerticalArrowHead��5*      $��13��6:   $��18��6F   
���  ��$��Bottom=�6\   +������- @B ���- @T� �@B `�S ���- @B 	 �� Stretch$
 Fill=�6�   $ 0.5,0.5&�6�   z�5+      V� 5,   "   .~�5-   &   �� �� 5.   &   �� 5/   &    �� Angle$ 90Q�66   T� 50   &   51   #   53      �� 54      
���  ��$��32��6/   $ 8��6;   $ 14��6K   $��2��6[   55      �� 56      $��46��6   $ 38��6)   $ 23��6:   $��18��6J   $ 0.5,0.5&�6U   z�57      V� 58      .~�59   "   �� �� 5:   "   �� 5;   "   $ 90Q�62   T� 5<   "   5=      .+�5?      F� -   5        $ VerticalArrowHead1��5?      $��13��6;   $��18��6G   
��� � �$��Bottom=�6_   +������- @B ���- @T� �@B `�S ���- @B 	$
 Fill=�6�   $ 0.5,0.5&�6�   z�5@      V� 5A   "   .~�5B   &   �� �� 5C   &   �� 5D   &   $ 90Q�66   T� 5E   &   5F   #   5H      �� 5I      
��� � �$��32��61   $ 8��6=   $ 14��6M   $��2��6]   5J      �� 5K      $��Z��6   $ 20��6(   
���  ����68   Z� 6N   �� 5L      $��Y��6   $ 28��6(   $ 46��69   
��� � ���6I   Z� 6_   5M      5N      5O      5P      5Q      