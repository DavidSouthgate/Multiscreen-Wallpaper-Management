# Multiscreen Wallpaper Management
> **I currently use only 1 monitor on my primary machine so this project is temporarilly on hold. For now.**

This allows wallpapers to be managed in a multiple monitor setup. If resolution and configuration changes the program will recalculate the wallpaper.

E.g. If the user uses WIN+P to adjust monitor configuration. The wallpaper will be adjusted so that it still looks correct.

## Requirements
* <a href="http://www.newtonsoft.com/json">Json.NET</a> </br>
Popular high-performance JSON framework for .NET 

## Known Issues and Constraints
* Vertical monitor configurations are not currently supported. </br>
<b>Supported:</b></br>
<img src="https://i.imgur.com/alXlsn1.png"></img> </br>
<b>Not Supported:</b></br>
<img src="https://i.imgur.com/MxD7reU.png"></img>

## Configuration
The configuration is stored in config.json in the directory of the executable.

```
{
	"screens":[
		{
			"name":"\\\\.\\DISPLAY2",
			"wallpaper":["Example1.png","Example10.png"],
			"padding_top":0
		},
		{
			"name":"\\\\.\\DISPLAY1",
			"wallpaper":["Example2.png"],
			"padding_top":56
		}
	],
	"misc":null,
}
```

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
    <td>Screen10.png</td>
    <td>Another wallpaper that would work best on \\.\DISPLAY2</td>
  </tr>
  <tr>
    <td>"padding_top":56</td>
    <td>DISPLAY1 is 56 pixels below DISPLAY2</td>
  </tr>
  <tr>
    <td>"padding_top":0</td>
    <td>DISPLAY2 is 0 pixels below top of configuration</td>
  </tr>
  <tr>
    <td>Screen2.png</td>
    <td>Wallpaper that would work best on \\.\DISPLAY1</td>
  </tr>
</table>
