class_name Card
extends TextureRect 

var cardSuit: String
var cardRank: String
var cardValue: int
var cardId: String
var cardTexture: String 


# Called when the node enters the scene tree for the first time.
func _ready():
	texture = load(cardTexture)
	print(cardSuit, cardRank, position, rotation)
	pass # Replace with function body.
	
func _gui_input(event):

	if event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_LEFT and event.pressed:
		print("Left mouse button was pressed!")	
	if event.is_action_released("LeftClick"):
		print(cardRank + ' of ' + cardSuit)
