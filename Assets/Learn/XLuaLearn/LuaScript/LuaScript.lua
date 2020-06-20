local util = require 'util'
local test1 = require 'NewFolder/test1'

util.hotfix_ex(CS.XLuaLoad,'Play',function(self)

	self:Play()
	--print("123123")

	print(">>>>>>>> main")
	require 'main'
end)


xlua.hotfix(CS.XLuaLoad,'Play1',function(self)	

	--print(">>>Play1")
end)

xlua.private_accessible(CS.XLuaLoad)--可以访问XLuaLoad中的非public变量
xlua.hotfix(CS.XLuaLoad,'Awake',function(self)

	print("lua:"..self._dict[1][1])
end)