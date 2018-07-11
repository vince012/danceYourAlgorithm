Dance Your Algorithm (DYA) is an different approach than traditionnal tools (Scratch) to learn programming for children.
A child makes movements, recognized by the Kinect camera, to create actions (ex : turn left). With these actions, the child can animate the sprite on
the scene. This version is quite basic, an implementation of conditions and variables are planned.

![DYA](https://raw.githubusercontent.com/vince012/danceYourAlgorithm/master/position_camera.PNG)

## Installation

Setting up project

	Copy projet, web and x86 folder from DYA_dependencies.zip to DYA/WpfApp1/bin/x86/Debug.

	Import DYA in your Visual Studio project, select WpfApp1 to x86 build mode and select WpfApp1 as starting project.
	Regenerate the solution if DYA doesn't start.

scratch-vm

	git clone https://github.com/LLK/scratch-vm.git
	cd scratch-vm
	npm install
	npm start
	
### Dependencies

NodeJS
	
## Features
### Gesture detection
        
Detects a gesture and add it to a list to create the sequence.

### Creation of a Scratch projet

A JavaScript function converts the sequence to Scratch blocs decribed in a JSON file. The project is a .sb3 archive
which contains the JSON file, images and sounds (sprite and scenes used).

### Viewing the sequence

The [scratch-vm](https://github.com/LLK/scratch-vm) is integrated to view the sequence created by the child.

### Drawing skeleton

Draws up to two skeletons.

### Variable input

A gesture is dedicated to open a text box to store a variable. Further improvement is to use a keypad to
bring up this textbox.

## Contributors

* Anas KARDY
* Clément DEMESLAY

## Credits

Some parts of gesture recognition system are based on [Vitruvius](https://github.com/LightBuzz/Vitruvius) conception.
