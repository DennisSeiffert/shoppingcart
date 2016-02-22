class InitializationError(Exception):
    pass


class StateMachine:
    def __init__(self):
        self.handlers = {}
        self.startState = None
        self.endStates = []
        self.currentHandler = None

    def add_state(self, name, handler, end_state=0):
        name = name.upper()
        self.handlers[name] = handler
        if end_state:
            self.endStates.append(name)

    def set_start(self, name):
        self.startState = name.upper()
        try:
            self.currentHandler = self.handlers[self.startState]
        except:
            raise InitializationError("must call .set_start() before .run()")
        if not self.endStates:
            raise  InitializationError("at least one state must be an end_state")

    def run(self, cargo, execute):
        newState = self.currentHandler(cargo)
        if newState.upper() in self.endStates:
            print("reached ", newState)
            execute()
        else:
            self.currentHandler = self.handlers[newState.upper()]