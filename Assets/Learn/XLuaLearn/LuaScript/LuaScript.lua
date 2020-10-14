local util = require 'util'

xlua.hotfix(CS.XLuaTest1,"Foo",nil)
util.hotfix_ex(CS.XLuaTest1,"Foo",function(self)
	self:Foo()
	print("Foo Lua")
end)


xlua.hotfix(CS.XLuaTest1,"Foo1",function(self, t)
	
	print(t * 100)

	local dict  = self._dict
	for k,v in pairs(dict) do
		print(k,v)
	end
end)