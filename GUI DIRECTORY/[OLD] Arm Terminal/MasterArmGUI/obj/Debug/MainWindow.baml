   M S B A M L   `   `   `  ����A  =Macros, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:Macros;assembly=MacrosMacros  5   	   E AArmTopView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:ArmTopView;assembly=ArmTopView
ArmTopView 6=   F BArmSideView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:ArmSideView;assembly=ArmSideViewArmSideView 6}   F BGUIArmDrive, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:GUIArmDrive;assembly=GUIArmDriveGUIArmDrive 6�   I EControl_Status, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:Control_Status;assembly=Control_StatusControl_Status 6  L HArmVideoComponent, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:ArmVideoComponent;assembly=ArmVideoComponentArmVideoComponent 6O  F BcommFeedViz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:commFeedViz;assembly=commFeedVizcommFeedViz 6(   MasterArmGUI   MasterArmGUI.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R	 NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W
 SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation  	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml   	 
 5   	   7Macros$clr-namespace:Macros;assembly=Macros   	 
 5   	   C
ArmTopView,clr-namespace:ArmTopView;assembly=ArmTopView   	 
 6=   FArmSideView.clr-namespace:ArmSideView;assembly=ArmSideView   	 
 6}   FGUIArmDrive.clr-namespace:GUIArmDrive;assembly=GUIArmDrive   	 
 6�   OControl_Status4clr-namespace:Control_Status;assembly=Control_Status   	 
 6  XArmVideoComponent:clr-namespace:ArmVideoComponent;assembly=ArmVideoComponent   	 
 6O  @d2http://schemas.microsoft.com/expression/blend/2008   	 
 6�  Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006   	 
 6�  FcommFeedViz.clr-namespace:commFeedViz;assembly=commFeedViz   	 
 6(  -   5          � Title$  
MainWindow��5   	   $
��1080��6   $
��1920��6*    � WindowState$ 	Maximized=�67   .��5      (�  (� Stretch$
 Fill=�6   .��5   
   � $
��1050��6   $
��1912��6   .+�5      � $
��Left=�6   $	��525��6.   $	��Top=�6;   $	��960��6S   .+�5	      $  ArmVideoComponent.armVideoComp  -   5          Name$ leftVid��5	   1   $
��Left=�6B   $	��Top=�6]   5
      � 5      $��Right=�6   $	��525��6/   $	��Top=�6<   $	��960��6T   .+�5        -   5        $ rightVid��5   1   $	��Top=�6C   5         Macros.Macros  5      -   5          Name$ macroButtons��5      $��Right=�62   $��Bottom=�6N   $��262.5��6i   $	��480��6x     ArmTopView.ArmTop  5      -   5          Name$ 
armTopView��5       $��Right=�64   $��Bottom=�6P   $	��263��6k   $	��480��6x   $��0,0,0,262.5q�6�     ArmSideView.ArmSide  5      -   5          Name$ armSideView��5   "   $��Right=�67   $��Bottom=�6S   $	��263��6n   $	��480��6{   $��0,0,480,262.5q�6�     GUIArmDrive.GUIArmDrive  5      -   5          Name$ GUIArmDrive��5   &   $��Right=�6;   $��	0,0,480,0q�6W   $��Bottom=�6j   $��262.5��6�   $	��480��6�   "  Control_Status.ControlStatus  5      -   5          Name$ ControlStatusPanel��5   +   $��Right=�6G   $��0,0,960,202q�6c   $��Bottom=�6x   $	��322��6�   $	��323��6�   (� 5      $��0,525,1274,202q�6   $
 Fill=�6.   .��5      � $	��480��6   $	��638��6$   .+�5        commFeedViz.commFeedViz  -	   5        	  Name$	 incomingCOMViz��5   .   $
��Left=�6F   $	��Top=�6a   $	��320��6y     5      -
   5        $	 outgoingCOMViz��5   .   $
��Left=�6F   $��	320,0,0,0q�6a   $	��Top=�6t   $	��318��6�     5      -   5        $	 incomingNETViz��5   .   $
��Left=�6F   $��	0,240,0,0q�6a   $	��Top=�6t   $	��320��6�     5      -   5        $	 outgoingNETViz��5   .   $
��Left=�6F   $��320,240,0,0q�6a   $	��Top=�6v   $	��318��6�   5       5!      (� 5"      $
��Left=�6   $��	0,848,0,0q�61   $	��Top=�6D   $	��638��6\   $	��202��6h   $
 Fill=�6u   .��5#        $	��252��6*   $	��638��67   5%      �� 5&      -   5        
 ��Name$
 localCamButton��5&      $��	Local Cam��6-   $
��Left=�6A   $��823,1010,0,0q�6\   $	��Top=�6r   $	��124��6�   $��40��6�   ��5'      �� 5(       �� EndPoint$	 0,1&�6*    �� 
StartPoint$	 0,0&�69   .��5)      �  � Color$ Black��6'    � Offset$ 0Q�65   � 5*      $ 	#FF19CEE0��6'   $ 0.526Q�69   � 5+      $ 	#FFE80A0A��6'   $ 1Q�69   5,      5.      50      51      52      