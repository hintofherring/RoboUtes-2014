   M S B A M L   `   `   `  ����J  FtoggleIndicator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:toggleIndicator;assembly=toggleIndicatortoggleIndicator  5   	   F BcommFeedViz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:commFeedViz;assembly=commFeedVizcommFeedViz 6X   J FsafeCommFeedViz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:safeCommFeedViz;assembly=safeCommFeedVizsafeCommFeedViz 6�    
rocOnboard   rocOnboard.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   RtoggleIndicator6clr-namespace:toggleIndicator;assembly=toggleIndicator     5   	   FcommFeedViz.clr-namespace:commFeedViz;assembly=commFeedViz     6X   RsafeCommFeedViz6clr-namespace:safeCommFeedViz;assembly=safeCommFeedViz     6�   -   5          � Title$  
MainWindow��5   	   $	��602��6   $��1471.43��6)   .��5      � .+�5   
   �� $��	DRIVE IP:��6   $
��Left=�6$   $	��Top=�6?   $��	10,10,0,0q�6W   $��45��6j   $	��100��6v   $��20(�6�   �� 5   
   -   5         ��Name$ driveIPLabel��5      $��NA��6&   $
��Left=�63   $	��Top=�6N   $��
154,10,0,0q�6f   $��48��6z   $	��158��6�   $��20(�6�   $   toggleIndicator.ToolboxControl  5
   
   -   5          Name$ driveConnectedInd��5
   )   $
��Left=�6D   $��
317,10,0,0q�6_   $	��Top=�6s   $��35��6�   $��33��6�   �� 5   
   $��ARM IP:��6   $
��Left=�6"   $	��Top=�6=   $��	10,50,0,0q�6U   $��45��6h   $	��100��6t   $��20(�6�   �� 5   
   -   5        $ 
armIPLabel��5      $��NA��6$   $
��Left=�61   $	��Top=�6L   $��
154,50,0,0q�6d   $��48��6x   $	��158��6�   $��20(�6�     5   
   -   5        $ armConnectedInd��5   )   $
��Left=�6B   $��
317,50,0,0q�6]   $	��Top=�6q   $��35��6�   $��33��6�   �� 5   
   $��LOGISTICS IP:��6   $
��Left=�6(   $	��Top=�6C   $��
10,100,0,0q�6[   $��35��6o   $	��121��6{   $��15(�6�   �� 5   
   -   5        $ logisticsIPLabel��5      $��NA��6*   $
��Left=�67   $	��Top=�6R   $��
154,90,0,0q�6j   $��48��6~   $	��158��6�   $��20(�6�     5   
   -   5        $ logisticsConnectedInd��5   )   $
��Left=�6H   $��
317,90,0,0q�6c   $	��Top=�6w   $��35��6�   $��33��6�   $  safeCommFeedViz.ToolboxControl  5   
   -   5          Name$ incomingInternet��5   )   $
��Left=�6C   $��	357,0,0,0q�6^   $	��Top=�6q   $	��280��6�   $	��273��6�      title$ Internet IN��6�     5   
   -	   5        $ driveFrontCOMIN��5   )   $
��Left=�6B   $��	630,0,0,0q�6]   $	��Top=�6p   $	��280��6�   $	��273��6�   $ Drive Front COM IN��6�     5   
   -
   5        $ driveBackCOMIN��5   )   $
��Left=�6A   $��	908,0,0,0q�6\   $	��Top=�6o   $	��280��6�   $	��273��6�   $ Drive Back COM IN��6�     5   
   -   5        $ panTiltCOMIN��5   )   $
��Left=�6?   $��1181,0,-67,0q�6Z   $	��Top=�6p   $	��280��6�   $	��273��6�   $ Pan Tilt COM IN��6�   �� 5   
   $��ENGINEERING IP:��6   $
��Left=�6*   $	��Top=�6E   $��
10,140,0,0q�6]   $��35��6q   $	��139��6}   $��15(�6�   �� 5   
   -   5        $ engineeringIPLabel��5      $��NA��6,   $
��Left=�69   $	��Top=�6T   $��154,130,0,0q�6l   $��48��6�   $	��158��6�   $��20(�6�     5   
   -   5        $ engineeringConnectedInd��5   )   $
��Left=�6J   $��317,130,0,0q�6e   $	��Top=�6z   $��35��6�   $��33��6�     5   
   -   5        $ armCOMIN��5   )   $
��Left=�6;   $��357,280,0,-33q�6V   $	��Top=�6m   $	��280��6�   $	��273��6�   $ 
Arm COM IN��6�     5   
   -   5        $ 
wristCOMIN��5   )   $
��Left=�6=   $��630,280,0,0q�6X   $	��Top=�6m   $	��280��6�   $	��273��6�   $ Wrist COM IN��6�     5   
   -   5        $ 	miscCOMIN��5   )   $
��Left=�6<   $��908,280,0,0q�6W   $	��Top=�6l   $	��280��6�   $	��273��6�   $ Misc COM IN��6�   �� 5   
   -   5        $
��DOWN��5      $
��Left=�6    $��1276,332,0,0q�6;   $	��Top=�6Q   $��75��6i   6t   �� 5   
   -   5        $��UP��5      $
��Left=�6   $��1276,357,0,0q�69   $	��Top=�6O   $��75��6g   6r   �� 5   
   -   5        $
��STOP��5      $
��Left=�6    $��1276,382,0,0q�6;   $	��Top=�6Q   $��75��6i   6t   5       5!      