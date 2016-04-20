@echo off
adb -s %1 shell mkdir /sdcard/test 
adb -s %1 shell rm /sdcard/test/*.* 
adb -s %1 push %2 /sdcard/test/test.xml 
adb -s %1 push %3 /data/local/tmp/ 
adb -s %1 shell uiautomator runtest /data/local/tmp/CalculatorAutoTest.jar -c xzy.test.uiautomator.Executive 
adb -s %1 pull /sdcard/test/ %4 