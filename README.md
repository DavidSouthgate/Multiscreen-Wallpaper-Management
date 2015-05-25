# Multiscreen Wallpaper Management
This allows wallpapers to be managed in a multiple monitor setup. If resolution and configuration changes the program will recalculate the wallpaper.

E.g. If the user uses WIN+P to adjust monitor configuration. The wallpaper will be adjusted so that it still looks correct.

## Known Issues and Constraints
* Vertical monitor configurations are not currently supported. </br>
<b>Supported:</b></br>
<img src="https://i.imgur.com/alXlsn1.png"></img> </br>
<b>Not Supported:</b></br>
<img src="https://i.imgur.com/MxD7reU.png"></img>

* The location of the top edge of all monitors must be the same.</br>
<b>Supported:</b></br>
<img src="https://i.imgur.com/alXlsn1.png"></img> </br>
<b>Not Supported:</b></br>
<img src="https://i.imgur.com/mhNOmVg.png"></img>

## Requirements
* <a href="http://www.newtonsoft.com/json">Json.NET</a> </br>
Popular high-performance JSON framework for .NET 

##Configuration
The configuration is stored in config.json in the directory of the executable.

[</br>
&nbsp;&nbsp;&nbsp;[</br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"\\\\.\\DISPLAY2",</br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"\\\\.\\DISPLAY1"</br>
&nbsp;&nbsp;&nbsp;],</br>
&nbsp;&nbsp;&nbsp;[</br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"Screen1.png",</br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"Screen2.png"</br>
&nbsp;&nbsp;&nbsp;]</br>
]

<table border="1">
  <tr>
    <td>\\.\DISPLAY2</td>
    <td>Indicates screen at the left most position</td>
  </tr>
  <tr>
    <td>\\.\DISPLAY1</td>
    <td>Indicates screen at the next left most position</td>
  </tr>
  <tr>
    <td>Screen1.png</td>
    <td>Wallpaper that would work best on \\.\DISPLAY2</td>
  </tr>
  <tr>
    <td>Screen2.png</td>
    <td>Wallpaper that would work best on \\.\DISPLAY1</td>
  </tr>
</table>
