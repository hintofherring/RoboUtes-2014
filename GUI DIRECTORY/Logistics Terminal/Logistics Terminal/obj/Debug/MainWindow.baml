   M S B A M L   `   `   `  ����I  EsafeMapPalette, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:safeMapPalette;assembly=safeMapPalettesafeMapPalette  5   	   Q MnetworkStatusIndicator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_Dclr-namespace:networkStatusIndicator;assembly=networkStatusIndicatornetworkStatusIndicator 6U   J FsafeCommFeedViz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:safeCommFeedViz;assembly=safeCommFeedVizsafeCommFeedViz 6�    Logistics Terminal#   Logistics_Terminal.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   OsafeMapPalette4clr-namespace:safeMapPalette;assembly=safeMapPalette     5   	   gnetworkStatusIndicatorDclr-namespace:networkStatusIndicator;assembly=networkStatusIndicator     6U   RsafeCommFeedViz6clr-namespace:safeCommFeedViz;assembly=safeCommFeedViz     6�   -   5          � Title$  
MainWindow��5   	   $
��1080��6   $
��1920��6*   .��5      (�  (� Stretch$
 Fill=�6   .��5   
   � ���	#FF383838$
��1050��6&   $
��1912��64   .+�5      �� $��10q�6   ���	#FFF70000$	��Top=�6B   $��652,0,650,0q�6Z   $	��610��6o   $	��390��6{   $��Center=�6�   |�. ._�5	      �� -   5         ��Name$ 
frontImage��5	      $��Stretch=�6,   $��Stretch=�6J   $��Resources/olymicPark.jpg��6f   5
      �� 5      $��10q�6   ���	#FFF70000$	��Top=�6B   $��1292,394,10,0q�6Z   $	��610��6q   $	��390��6}   $��Center=�6�   ._�5      �� -   5        $ 
rightImage��5      $��Stretch=�6,   $��Stretch=�6J   5      �� 5      $��10q�6   ���	#FFF70000$	��Top=�6B   $��652,650,650,0q�6Z   $	��610��6q   $	��390��6}   $��Center=�6�   ._�5      �� -   5        $ 	rearImage��5      $��Stretch=�6+   $��Stretch=�6I   5      �� 5      $��10q�6   ���	#FFF70000$	��Top=�6B   $��10,394,1292,0q�6Z   $	��610��6q   $	��390��6}   $��Center=�6�   ._�5      �� -   5        $ 	leftImage��5      $��Stretch=�6+   $��Stretch=�6I   5      �� 5      -   5        $ roverPic��5      $��Center=�6&   $	��530��6C   $��581,247,451,273q�6P   $��Center=�6i   $	��880��6�   $��Resources/transparent.png��6�    J� Opacity$	 0.3Q�6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$	 180Q�6*   T� 5      5      5       #   safeMapPalette.ToolboxControl  5"      -   5          Name$ 
mapPalette��5"   ,   $
��Left=�6@   $��10,0,0,0q�6[   $	��Top=�6m   $	��379��6�   $	��381��6�   �� 5#      -   5         ��Name$ frontRectangle��5#      $
��Left=�60   $	��390��6K   $��	652,0,0,0q�6X   
���   �$	��Top=�6z   $	��610��6�    � ContextMenu 5$      �� 5%      .@�5&      m� -	   5        $��Request New Image��5&   #   
	 J� Uid$	 newFront��6U   m� 5'      -
   5        $��Magnify - Left��5'   #   $	 	magFrontL��6R   m� 5(      -   5        $��Magnify - Right��5(   #   $	 	magFrontR��6S   5)      ��5+      �� 5,      
 �� Color$
 	#FFF4F4F5��6&    �� Opacity$ 0Q�68   5.      �� 5/      -   5        $ rightRectangle��5/      $
��Left=�60   $	��390��6K   $��1292,394,0,0q�6X   
���   �$	��Top=�6}   $	��610��6�    50      �� 51      .@�52      m� -   5        $��Request New Image��52   #   $	 newRight��6U   m� 53      -   5        $��Magnify - Left��53   #   $	 	magRightL��6R   m� 54      -   5        $��Magnify - Right��54   #   $	 	magRightR��6S   55      ��57      �� 58      $
 	#FFF4F4F5��6&   $ 0Q�68   5:      �� 5;      -   5        $ rearRectangle��5;      $
��Left=�6/   $	��390��6J   $��652,650,0,0q�6W   
���   �$	��Top=�6{   $	��610��6�    5<      �� 5=      .@�5>      m� -   5        $��Request New Image��5>   #   $	 newRear��6U   m� 5?      -   5        $��Magnify - Left��5?   #   $	 magRearL��6R   m� 5@      -   5        $��Magnify - Right��5@   #   $	 magRearR��6S   5A      ��5C      �� 5D      $
 	#FFF4F4F5��6&   $ 0Q�68   5F      �� 5G      -   5        $ leftRectangle��5G      $
��Left=�6/   $	��390��6J   $��
10,394,0,0q�6W   
���   �$	��Top=�6z   $	��610��6�    5H      �� 5I      .@�5J      m� -   5        $��Request New Image��5J   #   $	 newLeft��6U   m� 5K      -   5        $��Magnify - Left��5K   #   $	 magLeftL��6R   m� 5L      -   5        $��Magnify - Right��5L   #   $	 magLeftR��6S   5M      ��5O      �� 5P      $
 	#FFF4F4F5��6&   $ 0Q�68   5R      �� 5S      -   5         ��Name$ centerMapButton��5S      $��CENTER MAP ON JSC��6.   $
��Left=�6J   $	��Top=�6e   $	��141��6}   $��	391,0,0,0q�6�   $��41��6�   6�   +  %networkStatusIndicator.ToolboxControl  5U      -   5          Name$ networkConnectionInd��5U   4   $
��Left=�6R   $	��Top=�6m   $��92��6�   $��1615,958,0,0q�6�   $	��297��6�   $  safeCommFeedViz.ToolboxControl  5W      -   5          Name$ internetINViz��5W   -   $
��Left=�6D   $��
1262,0,0,0q�6_   $	��Top=�6s   $	��391��6�   $	��381��6�      title$ Internet IN��6�   �� 5X      -   5        $ clearMapButton��5X      $��CLEAR MAP OF ALL PINS��6-   $
��Left=�6M   $	��Top=�6h   $	��141��6�   $��
391,41,0,0q�6�   $��41��6�   6�   �� 5Y      -   5        $ 
tempButton��5Y      $
��TEMP��6)   $
��Left=�68   $	��Top=�6S   $	��141��6k   $��450,257,0,0q�6w   $��41��6�   6�   5[      5\      5]      