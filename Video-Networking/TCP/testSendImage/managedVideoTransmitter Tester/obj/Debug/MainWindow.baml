   M S B A M L   `   `   `  ����"  managedVideoTransmitter Tester/    )managedVideoTransmitter_Tester.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5      � .+�5   
   �� -   5         ��Name$ startButton��5      $��START��6&   $
��Left=�66   $��
352,61,0,0q�6Q   $	��Top=�6e   $��75��6}   6�   �� 5   
   -   5        $ 
stopButton��5      $
��STOP��6%   $
��Left=�64   $��352,103,0,0q�6O   $	��Top=�6d   $��75��6|   6�   �� 5   
   -   5         ��Name$ qualitySlider��5      $
��Left=�6(   $��
10,202,0,0q�6C   $	��Top=�6W   $	��497��6o    � Maximum$	 100Q�6{    � SmallChange$ 1Q�6�    �� TickPlacement$
 Both=�6�    �� IsSnapToTickEnabled . 6�   �� 5	   
   $��Quality% : ��6   $
��Left=�6&   $��356,132,0,0q�6A   $	��Top=�6V   �� 5
   
   -   5         ��Name$ qualityLabel��5
      $��NA��6&   $
��Left=�63   $��428,132,0,0q�6N   $	��Top=�6c   �� 5   
   $��QUALITY��6   $
��Left=�6"   $��
10,171,0,0q�6=   $	��Top=�6Q   �� 5   
   -   5        $ 	fpsSlider��5      $
��Left=�6$   $��
10,263,0,0q�6?   $	��Top=�6S   $	��497��6k   $ 50Q�6w   $ 1Q�6�   $
 Both=�6�    . 6�   �� 5   
   $	��FPS��6   $
��Left=�6   $��
10,232,0,0q�69   $	��Top=�6M   �� 5   
   $��	set FPS: ��6   $
��Left=�6$   $��342,158,0,0q�6?   $	��Top=�6T   �� 5   
   -   5        $ fpsLabel��5      $��NA��6"   $
��Left=�6/   $��396,158,0,0q�6J   $	��Top=�6_   $��80��6w   �� 5   
   $��actual transmitted FPS:��6   $
��Left=�62   $��
17,132,0,0q�6M   $	��Top=�6a   �� 5   
   -   5        $ actualFPSLabel��5      $��NA��6(   $
��Left=�65   $��153,132,0,0q�6P   $	��Top=�6e   $��80��6}   5      5      