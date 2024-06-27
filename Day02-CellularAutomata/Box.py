class Box:
    # Static property
    size = 0.4  # Size of the box
    
    # Class-level variables to track counts
    total_number = 0
    num_boxes_grid = 0
    num_boxes_queue = 0
    instances = []  # List to keep track of all box instances

    # Valid categories for the box
    VALID_CATEGORIES = {'grid', 'queue'}
    # Valid types for the box
    VALID_TYPES = {'special', 'normal'}
    # Valid sound sensors for the box
    VALID_SENSORS = {'SLS_01', 'SLS_02', 'SLS_03'}

    def __init__(self, position, category, box_type="normal", sound_level=None, sound_sensor=None):
        # Validate and set the category of the box
        if category not in self.VALID_CATEGORIES:
            raise ValueError(f"Invalid category '{category}'. Must be one of: {', '.join(self.VALID_CATEGORIES)}")
        self.category = category  # Category of the box ('grid' or 'queue')

        # Validate and set the type of the box
        if box_type not in self.VALID_TYPES:
            raise ValueError(f"Invalid box type '{box_type}'. Must be one of: {', '.join(self.VALID_TYPES)}")
        self.type = box_type  # Type of the box ('special' or 'normal')
        
        self.position = position  # Position of the box in 3D space (Rhino.Geometry.Point3d)
        
        if self.type == 'special':
            self.sound_level = sound_level  # Sound level value for special boxes
            if sound_sensor not in self.VALID_SENSORS:
                raise ValueError(f"Invalid sound sensor '{sound_sensor}'. Must be one of: {', '.join(self.VALID_SENSORS)}")
            self.sound_sensor = sound_sensor  # Set the sound sensor for special boxes
        else:
            self.sound_level = None
            self.sound_sensor = None  # No sound sensor for 'normal' boxes
        
        # Increment static counters based on category
        Box.total_number += 1
        if category == 'grid':
            Box.num_boxes_grid += 1
        elif category == 'queue':
            Box.num_boxes_queue += 1
        
        Box.instances.append(self)  # Add instance to the list of instances

    @staticmethod
    def clear_instances():
        # Reset static counters
        Box.total_number = 0
        Box.num_boxes_grid = 0
        Box.num_boxes_queue = 0
        # Optionally, clear any other state or lists of instances if needed
        Box.instances = []  # Clear the list of instances
        
    def __del__(self):
        # Optionally implement a destructor to manage instance deletion
        pass