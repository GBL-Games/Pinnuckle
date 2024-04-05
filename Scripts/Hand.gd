extends Node2D 

@export var handOwner: String

var currentHand: Array

const cardSize = Vector2(23, 34)

@onready var card = preload("res://Objects/Card.tscn")
@onready var spreadCurve = preload("res://Resources/HandSpreadCurve.tres")
@onready var radius = 20
@onready var angleRange = 90 
@onready var tiltAngle = 10
@onready var cardSpacing = 20

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
	var totalCards = currentHand.size()

	# loop over hands.
	# instantiate and position cards in an arch
	for i in range(totalCards):
		var handPositionRatio := 0.5
		var destination := position
		destination.x += spreadCurve.sample(handPositionRatio) * cardSpacing
		
		var cardInstance = card.instantiate();
		
		if handOwner == "player":
			cardInstance.set_frame(currentHand[i].spriteIndex)
			cardInstance.cardRank = currentHand[i].rank
			cardInstance.cardSuit = currentHand[i].suit
			cardInstance.cardValue = currentHand[i].value
		else: 
			cardInstance.cardTexture = 44 
	   
		if totalCards > 1:
			handPositionRatio = float(i)/float(totalCards - 1)

		add_child(cardInstance)
		pass
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
# func _process(delta):
# 	pass
