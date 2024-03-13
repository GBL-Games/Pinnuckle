extends Node2D

@export var card_suit: String
@export var card_rank: String
@export var card_id: String
@export var sprite_id: int = 0


# Called when the node enters the scene tree for the first time.
func _ready():
	$CardSprite.frame = sprite_id
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
