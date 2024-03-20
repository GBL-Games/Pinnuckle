extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	print("Start match")
	# Shuffle Cards
	SignalBus.emit_signal("shuffleCards")
	
	# Deal Cards
	SignalBus.emit_signal("dealCards", 12, "player")
	SignalBus.emit_signal("dealCards", 12, "opponent")
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
# func _process(delta):
# pass
