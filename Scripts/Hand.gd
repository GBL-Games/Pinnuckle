extends PanelContainer 

@export var handOwner: String

var currentHand: Array

const cardSize = Vector2(23, 34)

@onready var card = preload("res://Objects/Card.tscn")
@onready var centerCardOval = Vector2(size.x, size.y) * Vector2(0.5, 1.25)
@onready var horizontalRadius = size.x * 0.45
@onready var verticalRadius = size.y * 0.4

var angle = deg_to_rad(90) - 0.5
var ovalAngleVector = Vector2()


# Called when the node enters the scene tree for the first time.
func _ready():
	SignalBus.connect("giveCards", addToHand)
	pass 

func addToHand(cardsOwner:String, cards:Array):
	if cardsOwner == handOwner: 
		currentHand.append_array(cards)
		_updateHand()	
	pass

func _updateHand():
	for i in range(currentHand.size()):
		var cardInstance = card.instantiate();
		if handOwner == "player":
			cardInstance.cardTexture = currentHand[i].cardTexture
			cardInstance.cardRank = currentHand[i].rank
			cardInstance.cardSuit = currentHand[i].suit
			cardInstance.cardValue = currentHand[i].value
		else: 
			cardInstance.cardTexture = "res://Assets/Textures/Backs/CardBack4.png"
		
		ovalAngleVector = Vector2(horizontalRadius * cos(angle), - verticalRadius * sin(angle))
		cardInstance.position = centerCardOval + ovalAngleVector - cardInstance.size/2
		cardInstance.scale *= cardSize/cardInstance.size
		cardInstance.rotation = (90 - rad_to_deg(angle))/12

		angle += 0.25

		add_child(cardInstance)
		pass
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
# func _process(delta):
# 	pass
