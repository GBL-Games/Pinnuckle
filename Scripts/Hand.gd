extends Node2D 

@export var handOwner: String

var currentHand: Array

const cardSize = Vector2(23, 34)

@onready var card = preload("res://Objects/Card.tscn")
@onready var angle = 0
@onready var radius = 100
var increment:float

# Called when the node enters the scene tree for the first time.
func _ready():
	SignalBus.connect("giveCards", addToHand)
	pass 

func addToHand(cardsOwner:String, cards:Array):
	if cardsOwner == handOwner: 
		currentHand.append_array(cards)
		increment = 180 / currentHand.size()
		_updateHand()	
	pass

func _updateHand():
	for i in range(currentHand.size()):
		var cardInstance = card.instantiate();
		if handOwner == "player":
			cardInstance.set_frame(currentHand[i].spriteIndex)
			cardInstance.cardRank = currentHand[i].rank
			cardInstance.cardSuit = currentHand[i].suit
			cardInstance.cardValue = currentHand[i].value
		else: 
			cardInstance.cardTexture = 44 

		var x = cos(deg_to_rad(angle)) * radius
		var y = sin(deg_to_rad(angle)) * radius / 2

		cardInstance.position = Vector2(x, y) 
		#cardInstance.scale *= cardSize/cardInstance.size
		cardInstance.rotation = rad_to_deg(angle)/12

		angle -= increment

		add_child(cardInstance)
		pass
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
# func _process(delta):
# 	pass
