   M S B A M L   `   `   `  ����J  FtoggleIndicator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:toggleIndicator;assembly=toggleIndicatortoggleIndicator  5   	   F BcommFeedViz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:commFeedViz;assembly=commFeedVizcommFeedViz 6X    wristControlDevelopment(   "wristControlDevelopment.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   RtoggleIndicator6clr-namespace:toggleIndicator;assembly=toggleIndicator     5   	   FcommFeedViz.clr-namespace:commFeedViz;assembly=commFeedViz     6X   -   5          � Title$  
MainWindow��5   	   $	��350��6   $	��681��6)   .��5      � .+�5   
   �� -   5         ��Name$ spinHand��5      
���  ��$
��Left=�61   $��6��6L   $��211,165,0,0q�6W   
���   �$	��Top=�6{   $��53��6�    J� RenderTransformOrigin$ 0,0.5&�6�   �� 5   
   $��Rotation Angle: ��6   $
��Left=�6+   $��
84,258,0,0q�6F   $	��Top=�6Z   �� 5	   
   -   5         ��Name$ rotAngleLabel��5	      $��NA��6'   $
��Left=�64   $��184,258,0,0q�6O   $	��Top=�6d   $	��156��6|   �� 5
   
   $��Magnitude: ��6   $
��Left=�6&   $��
84,284,0,0q�6A   $	��Top=�6U   �� 5   
   -   5        $ magLabel��5      $��NA��6"   $
��Left=�6/   $��184,284,0,0q�6J   $	��Top=�6_   $	��156��6w   $   toggleIndicator.ToolboxControl  5   
   -   5          Name$ upInd��5   )   $
��Left=�68   $��
191,81,0,0q�6S   $	��Top=�6g   $��41��6   $��41��6�     5   
   -   5        $ leftInd��5   )   $
��Left=�6:   $��114,191,0,0q�6U   $	��Top=�6j   $��41��6�   $��41��6�     5   
   -   5        $ rightInd��5   )   $
��Left=�6;   $��267,191,0,0q�6V   $	��Top=�6k   $��41��6�   $��41��6�   �� 5   
   -   5        $ upPercentageLabel��5      $��NA��6+   $
��Left=�68   $��
274,81,0,0q�6S   $	��Top=�6g   �� 5   
   -	   5        $ rightPercentageLabel��5      $��NA��6.   $
��Left=�6;   $��345,191,0,0q�6V   $	��Top=�6k   �� 5   
   -
   5        $ leftPercentageLabel��5      $��NA��6-   $
��Left=�6:   $��180,191,0,0q�6U   $	��Top=�6j   �� 5   
   $	��%: ��6   $
��Left=�6   $	��Top=�69   $��155,191,0,0q�6Q   �� 5   
   $	��%: ��6   $
��Left=�6   $	��Top=�69   $��314,191,0,0q�6Q   �� 5   
   $	��%: ��6   $
��Left=�6   $	��Top=�69   $��
248,81,0,0q�6Q   �� 5   
   -   5        $ 
upMagLabel��5      $��NA��6$   $
��Left=�61   $��291,107,0,0q�6L   $	��Top=�6a   �� 5   
   $��MAG: ��6   $
��Left=�6    $	��Top=�6;   $��248,107,0,0q�6S   �� 5   
   -   5        $ rightMagLabel��5      $��NA��6'   $
��Left=�64   $��357,216,0,0q�6O   $	��Top=�6d   �� 5   
   $��MAG: ��6   $
��Left=�6    $	��Top=�6;   $��314,216,0,0q�6S   �� 5   
   -   5        $ leftMagLabel��5      $��NA��6&   $
��Left=�63   $��198,216,0,0q�6N   $	��Top=�6c   �� 5   
   $��MAG: ��6   $
��Left=�6    $	��Top=�6;   $��155,216,0,0q�6S   �� 5   
   -   5         ��Name$ maxMagSlider��5      $
��Left=�6'   $��
462,10,0,0q�6B   $	��Top=�6V   $	��201��6n    � Maximum$
 1024Q�6z    � SmallChange$ 0Q�6�    �� IsSnapToTickEnabled . 6�   �� 5   
   $��Max Magnitude:��6   $
��Left=�6)   $	��Top=�6D   $��
462,37,0,0q�6\   $��99��6p   �� 5   
   -   5        $ maxMagLabel��5      $��NA��6%   $
��Left=�62   $��
566,37,0,0q�6M   $	��Top=�6a   $��97��6y     commFeedViz.commFeedViz  5    
   -   5        	  Name$	 
wristInViz��5    "   $
��Left=�66   $��
389,81,0,0q�6Q   $	��Top=�6e   $	��229��6}   $	��274��6�   5"      5#      