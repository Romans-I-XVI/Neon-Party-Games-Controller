global.trackpad_width = obj_trackpad.trackpad_width
global.trackpad_x = obj_trackpad.x
global.trackpad_y = obj_trackpad.y
ini_open("savedata.ini");
    ini_write_real("data", "trackpad_width", obj_trackpad.trackpad_width); 
    ini_write_real("data", "trackpad_x", obj_trackpad.x); 
    ini_write_real("data", "trackpad_y", obj_trackpad.y); 
ini_close();
