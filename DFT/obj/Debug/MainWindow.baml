   M S B A M L   `   `   `  ����>  :DFT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:DFTDFT  5   	   g cLiveChartsCore.SkiaSharpView.WPF, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc92a3e13d40c573}Xclr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF LiveChartsCore.SkiaSharpView.WPF 5   	       DFT.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:DFT     5   	   hlvcXclr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF     5   	     � WindowStartupLocation$  CenterScreen=�5	   	    � Title$ 	Workspace��5
   	   $	��450��6   $	��800��6(    � 	Resources 5       �� 
TargetType �� Property �� Value��%�  'o�       'm� �     �� 5   
   " o�6   .�5      ��  � �� �#1f1f1f6+   �� 5       � ��
 �����6+   �� 5       � �� �#1f1f1f6,   �� 5       � ��$ 0q�60   5      �� 5   
   " m�6   .�5      ��  � �� �#1f1f1f6+   �� 5       � ��
 �����6+   �� 5       � �� �#1f1f1f6,   �� 5       � ��$ 0q�60   5      5      .��5      � ���#121212 � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��* �6   �� 5      $��* �6   �� 5       $��* �6   .+�5#   
   o� $��30��6   $	��Top=�6   .@�5$      m� $��Upload audio��6   $��30��6-   .@�5%      m� -   5        $��	Own audio��5%      6.   m� 5&      -   5         m�Name$" MenuItemUploadPreloadedAudio��5&      $��Default audio��6A   6X   5'      m� 5(      -   5        $ MenuItemAudioPlayer��5(      $��
Play audio��64   $��30��6H   5)      5*      5  /LiveChartsCore.SkiaSharpView.WPF.CartesianChart  5,   
   -   5          Name$ ChartAmplitude��5,      	   ZoomMode$
	 Both=�65   $��10,10,10,10q�6E   $��1��6Z   
   XAxes
 6g   �� *AmplitudeXAxes+   YAxes 6�   �� *AmplitudeYAxes+  5.   
   -   5        $ ChartFrequency��5.      $
	 Both=�65   ,  &LiveChartsCore.SkiaSharpView.WPF.Chart   TooltipPosition$	 Top=�6E   $��10,10,10,10q�6[   $��2��6p   
 6}   �� *FrequencyXAxes+ 6�   �� *FrequencyYAxes+  50   
   -   5        $ ChartSin��50      $
	 Both=�6/   $	 Top=�6?   $��10,10,10,10q�6U   $��3��6j   
 6w   �� *	SineXAxes+ 6�   �� *	SineYAxes+52      54      