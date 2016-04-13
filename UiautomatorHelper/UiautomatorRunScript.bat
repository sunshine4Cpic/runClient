@echo off
adb -s %1 shell mkdir /sdcard/test >nul
adb -s %1 shell rm /sdcard/test/*.* >nul
adb -s %1 push %2 /sdcard/test/test.xml >nul
adb -s %1 push %3 /data/local/tmp/ >nul
adb -s %1 shell uiautomator runtest /data/local/tmp/CalculatorAutoTest.jar -c xzy.test.uiautomator.Executive >nul
adb -s %1 pull /sdcard/test/ %4 >nul