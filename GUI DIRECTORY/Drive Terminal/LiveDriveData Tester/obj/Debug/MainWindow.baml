   M S B A M L   `   `   `  ����H  DLiveDriveData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:LiveDriveData;assembly=LiveDriveDataLiveDriveData  5   	    LiveDriveData Tester%   LiveDriveData_Tester.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   LLiveDriveData2clr-namespace:LiveDriveData;assembly=LiveDriveData     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5      � .+�5   
   "   LiveDriveData.ToolboxControl  -   5          Name$ liveDriveDataComp��5   '   $
��Left=�6B   $��	10,10,0,0q�6]   $	��Top=�6p   $	��272��6�   $	��241��6�   �� 5   
   -   5         ��Name$ 
stallLabel��5      $��Center=�6$   $��STALL��6H   $
��Left=�6X   $��
256,69,0,0q�6s   $	��Top=�6�   
���  ��$
��Bold"�6�   6�   �� 5	   
   -   5        $ 	slipLabel��5	      $��Center=�6#   $
��SLIP��6G   $
��Left=�6V   $��256,100,0,0q�6q   $	��Top=�6�   
���  ��$
��Bold"�6�   $��44��6�   6�   �� 5
   
   $��
PID Goal: ��6   $
��Left=�6%   $��256,193,0,0q�6@   $	��Top=�6U   $��62��6m   �� 5   
   $��Actual: ��6   $
��Left=�6#   $��256,219,0,0q�6>   $	��Top=�6S   $��62��6k   �� 5   
   $��Speed: ��6   $
��Left=�6"   $��256,250,0,0q�6=   $	��Top=�6R   $��62��6j   �� 5   
   $��	Current: ��6   $
��Left=�6$   $��256,281,0,0q�6?   $	��Top=�6T   $��62��6l   �� 5   
   -   5         ��Name$ pidGoalSlider��5      $
��Left=�6(   $��332,197,0,0q�6C   $	��Top=�6X   $	��139��6p   6|   �� 5   
   -   5        $ actualSlider��5      $
��Left=�6'   $��332,224,0,0q�6B   $	��Top=�6W   $	��139��6o   6{   �� 5   
   -   5        $ speedSlider��5      $
��Left=�6&   $��332,251,0,0q�6A   $	��Top=�6V   $	��139��6n   6z   �� 5   
   -   5        $ currentSlider��5      $
��Left=�6(   $��332,278,0,0q�6C   $	��Top=�6X   $	��139��6p   6|   �� 5   
   -   5        $ spinningLabel��5      $��Center=�6'   $
��SLIP��6K   $
��Left=�6Z   $��256,131,0,0q�6u   $	��Top=�6�   
���  ��$
��Bold"�6�   $��44��6�   6�   5      5      