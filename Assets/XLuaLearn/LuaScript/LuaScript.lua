local util = require 'util'
local _, LuaDebuggee = pcall(require, 'LuaDebuggee')
if LuaDebuggee and LuaDebuggee.StartDebug then
	if LuaDebuggee.StartDebug('127.0.0.1', 9826) then
		print('LuaPerfect: Successfully connected to debugger!')
	else
		print('LuaPerfect: Failed to connect debugger!')
	end
else
	print('LuaPerfect: Check documents at: https://luaperfect.net')
end

xlua.hotfix(CS.XLuaTest1,"Foo",nil)
util.hotfix_ex(CS.XLuaTest1,"Foo",function(self)
	self:Foo()
	print("Foo Lua")
end)


--xlua.hotfix(CS.XLuaTest1,"Foo1",function(self, t)
	
	--print(t * 100)

	--local dict  = self._dict
	--for k,v in pairs(dict) do
		--print(k,v)
	--end
--end)

-- 参数个数不同的重载lua可以区分
-- 参数类型不同的重载lua不可以区分 都会修改

--xlua.hotfix(CS.XLuaTest1,"Foo",function(self, t)

		--if(type(t) == 'number') then
			--print(t .. type(t).."[hot fix]")
		--else
			--self:Foo(t)
		--end
	--end)




--xlua.hotfix(CS.XLuaTest1,"Foo1",function(self, t)

		--print(t .. type(t).."[hot fix]")
		--CS.XLua.HotfixDelegateBridge.Set(1, function(self, t)

				--print(t .. type(t).."[hot fix]")

			--end)
--end)