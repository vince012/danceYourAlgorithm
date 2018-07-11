function go2(msg) {
	alert(msg);
}

class CustomeStage{
	constructor(){
		this.assetId= "cd21514d0531fdffb22204e0ec5ed84a";
		this.name= "backdrop1";
		this.md5ext= "cd21514d0531fdffb22204e0ec5ed84a.svg";
		this.dataFormat= "svg";
		this.rotationCenterX= 0;
		this.rotationCenterY= 0;
	}
}
class Custome{
	constructor(assetId, name, bitmapResolution, md5ext, dataFormat, rotationCenterX, rotationCenterY){
		this.assetId= assetId;
		this.name= name;
		this.bitmapResolution= bitmapResolution;
		this.md5ext= md5ext;
		this.dataFormat= dataFormat;
		this.rotationCenterX= rotationCenterX;
		this.rotationCenterY= rotationCenterY;
	}
}
class SoundStage{
	constructor(){
		this.assetId= "83a9787d4cb6f3b7632b4ddfebf74367";
		this.name= "pop";
		this.dataFormat= "wav";
		this.format= "";
		this.rate= 48000,
		this.sampleCount= 1032;
		this.md5ext= "83a9787d4cb6f3b7632b4ddfebf74367.wav";
	}
}
class Sound{
	constructor(assetId, name, dataFormat, format, rate, sampleCount, md5ext){
		this.assetId= assetId;
		this.name= name;
		this.dataFormat= dataFormat;
		this.format= format;
		this.rate= rate;
		this.sampleCount= sampleCount;
		this.md5ext= md5ext;
	}
}
/* BLOCK */
class BlockWithId{
	constructor(id, block){
		var idU = generateId();
		this.id = ( (id == 0) ? idU : id);
		this.block = block;
		if(id == 0) console.log(idU);
		else console.log(id);
	}
	next(next){
		if(this.block.acceptNext){
			this.block.next = next.id;
			next.block.parent = this.id;
		}
		else
			throw new Error("Block ga3ma kiAccepté next");
	}
	parent(parent){
		this.block.parent = parent.id;
		parent.block.next = this.id;
	}
	onlyParentById(id){
		this.block.parent = id;
	}
}
class Block{
	constructor(opcode, next, parent, inputs, fields, topLevel, shadow){
		this.opcode = opcode;
		this.next = next;
		this.parent = parent;
		this.inputs = inputs;
		this.fields = fields;
		this.topLevel = topLevel;
		this.shadow = shadow;
		this.acceptNext = true;
		Object.defineProperty(this, 'acceptNext', {
			enumerable: false
		});
	}
}
class BlockEvent extends Block{
	constructor(opcode, next, parent, inputs, fields, topLevel, shadow, x, y){
		super(opcode, next, parent, inputs, fields, topLevel, shadow);
		this.x = x;
		this.y = y;
	}
}
/* Event */
class FlagGreen extends BlockEvent{
	constructor(){
		super("event_whenflagclicked", null, null, {}, {}, true, false, 135, 129);
	}
}
/* Fin Event */
/* Motion */
class Changexby extends Block{
	constructor(distanceX){
		if(distanceX == null) distanceX = 10;
		var inputs =  {
			DX: [
				1,
				[4, distanceX.toString()]
			]
		};
		var fields =  {};
		super("motion_changexby", null, null, inputs, fields, false, false);
	}
}

class Changeyby extends Block{
	constructor(distanceX){
		if(distanceX == null) distanceX = 10;
		var inputs =  {
			DY: [
				1,
				[4, distanceX.toString()]
			]
		};
		var fields =  {};
		super("motion_changeyby", null, null, inputs, fields, false, false);
	}
}
class Gotoxy extends Block{
	constructor(X, Y){
		if(X==null||Y==null){
			X = -190;
			Y = -80;
		}
		var inputs = {
			X: [1, [4, X.toString()]],
			Y: [1, [4, Y.toString()]]
		};
		var fields = {};
		super("motion_gotoxy", null, null, inputs, fields, false, false);
	}
}
class TurnRight extends Block{
	constructor(degree){
		if(degree == null) degree = 15;
		var inputs = {
			DEGREES: [1, [4, degree.toString()]]
		};
		var fields = {};
		super("motion_turnright", null, null, inputs, fields, false, false);
	}
}
class TurnLeft extends Block{
	constructor(degree){
		if(degree == null) degree = 15;
		var inputs = {
			DEGREES: [1, [4, degree.toString()]]
		};
		var fields = {};
		super("motion_turnleft", null, null, inputs, fields, false, false);
	}
} 
/* Fin Motion */
/* Controls */
class ControlRepeat extends Block{
	constructor(time, arrayBlock, id){
		if(arrayBlock == null || !arrayBlock instanceof Array) throw new Error("Khask dakhel tableau fBlock dyal ControlRepeat");
		if(time==null || time <= 0) throw new Error("Valeur TIME dyal la boucle khasha tkon positive");
		if(id == null) throw new Error("Khask dakhel Id dyal loop bach najoutéwha l parent dyal bloc lawel");
		if(arrayBlock.length > 0)
			arrayBlock[0].onlyParentById(id);
		var inputs = {
			TIMES: [1, [6, time.toString()]],
			SUBSTACK: [(arrayBlock.length != 0 ) ? 2 : 1, (arrayBlock.length != 0 ) ? arrayBlock[0].id : null]
		};
		if(arrayBlock.length > 1){
			for(var i = 1; i < arrayBlock.length; i++)
				arrayBlock[i].parent(arrayBlock[i-1]);
		}
		var fields = {};
		super("control_repeat", null, null, inputs, fields, false, false);
	}
}
class ControlForever extends Block{
	constructor(arrayBlock, id){
		if(arrayBlock == null || !arrayBlock instanceof Array) throw new Error("Khask dakhel tableau fBlock dyal ControlRepeat");
		if(id == null) throw new Error("Khask dakhel Id dyal loop bach najoutéwha l parent dyal bloc lawel");
		if(arrayBlock.length == 0)
			var inputs = {};
		else{
			arrayBlock[0].onlyParentById(id);
			var inputs = {
				SUBSTACK: [2, arrayBlock[0].id]
			};
		}
		if(arrayBlock.length > 1){
			for(var i = 1; i < arrayBlock.length; i++)
				arrayBlock[i].parent(arrayBlock[i-1]);
		}
		var fields = {};
		super("control_forever", null, null, inputs, fields, false, false);
		this.acceptNext = false;
	}	
}
class ControlStop extends Block{
	constructor(option){
		var opt = (option == null) ? "all" : option;
		var inputs = {};
		var fields = {
			STOP_OPTION: [opt]
		};
		super("control_stop", null, null, inputs, fields, false, false);
		this.mutation = {
			tagName: "mutation",
			children: [],
			hasnext: "false"
		}
		this.acceptNext = false;
	}
}
class ControlWait extends Block{
	constructor(duration){
		if(duration == null) duration = 1;
		var inputs = {
			DURATION: [1, [5, duration.toString()]]
		};
		var fields = {};
		super("control_wait", null, null, inputs, fields, false, false);
	}
}
/* Fin Controle */

class Target{
	constructor(isStage, name){
		this.isStage = isStage;
		this.name = name;
		this.variables = (isStage ? {"`jEk@4|i[#Fk?(8x)AV.-my variable": ["my variable", 0]} : {});
		this.lists = {};
		this.broadcasts = {};
		this.blocks = {};
		this.currentCostume = 0;
		this.costumes = [(isStage ? new CustomeStage() : {} )];
		this.sounds = [(isStage ? new SoundStage() : {} )];
		this.volume = 100;
	}
}
class Stage extends Target{

	constructor(isStage, name){
		super(isStage, name);

		this.tempo = 60;
		this.videoTransparency = 50;
		this.videoStat = "off";
	}
}

class Sprite extends Target{
	constructor(isStage, name){
		super(isStage, name);
		this.visible = true;
		this.x = -190;
		this.y = -80;
		this.size = 100;
		this.direction = 90;
		this.draggable = false;
		this.rotationStyle = "all around";
	}
}

function go(){
	var stage = new Stage(true, "Stage");
	var sprite = new Sprite(false, "Sprite1");

	var custome1 = new CotumeStage();
	var custome2 = new CostumeStage();

	var meow = new SoundStage();
	sprite.costumes = [custome1, custome2];
	sprite.sounds = [meow];

	var id;
	var flag = new BlockWithId(0, new FlagGreen());
	var moveX = new BlockWithId(0, new Changexby(10));
	var moveY = new BlockWithId(0, new Changeyby(10));
	
	flag.next(reset);
	reset.next(loop);

	sprite.blocks = {
		[flag.id]: flag.block,
		[moveX.id]: moveX.block,
		[moveY.id]: moveY.block,
	}	

	var objet = {
		targets: [
			stage,
			sprite
		],
		meta: {
			semver: "3.0.0",
			vm: "0.1.0-prerelease.1526929817",
			agent: "Script WaiiM"
		}
	};
	document.getElementById('main').innerHTML = JSON.stringify(objet);
}

function generateId() {
  var text = "";
  var possible = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
  for (var i = 0; i < 20; i++)
    text += possible.charAt(Math.floor(Math.random() * possible.length));

  return text;
}
