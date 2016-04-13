adb -s %1 shell screencap -p /sdcard/screenshot.png
adb -s %1 pull /sdcard/screenshot.png %2
