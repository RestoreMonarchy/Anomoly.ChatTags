# Anomoly.ChatTags

RocketMod plugin to enhance player chat with dynamic, rich-text tags based on permissions

## Configuration

-   `ChatTags` - A list of chat tags to apply to players based on permissions.
    -   `ChatTag` - A chat tag to apply to players based on permissions.
        -   `Prefix` - The prefix to apply to the player's name.
        -   `Suffix` - The suffix to apply to the player's name.
        -   `Permission` - The permission required to apply the tag.
-   `ChatModePrefixes` - The prefixes to apply to the chat format based on chat mode.
    -   `World` - The prefix to apply when the player is in world chat mode.
    -   `Area` - The prefix to apply when the player is in area chat mode.
    -   `Group` - The prefix to apply when the player is in group chat mode.
-   `ChatFormats` - A list of chat formats to apply to players based on permissions.
    -   `ChatFormat` - A chat format to apply to players based on permissions.
        -   `Permission` - The permission required to apply the format.
        -   `Format` - The format to apply to the player's chat message.
        -   `UseRichText` - Whether or not to use rich text in the format.
-   `BaseColor` - The base color to apply to the player's name. Leave blank to use the default color. Ex `<BaseColor />`

```xml
<?xml version="1.0" encoding="utf-8"?>
<ChatTagPluginConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <ChatTags>
    <ChatTag permission="tag.admin">
      <Prefix>&lt;color=blue&gt;Admin&lt;/color&gt;</Prefix>
      <Suffix />
    </ChatTag>
    <ChatTag permission="tag.vip">
      <Prefix />
      <Suffix>&lt;color=yellow&gt;VIP&lt;/color&gt;</Suffix>
    </ChatTag>
  </ChatTags>
  <ChatFormats>
    <ChatFormat>
      <Permission>format.example_no_tags</Permission>
      <Format>[{CHAT_MODE}] {PLAYER_NAME}: {MESSAGE}</Format>
      <UseRichText>true</UseRichText>
    </ChatFormat>
  </ChatFormats>
  <ChatModePrefixes>
    <World>W</World>
    <Area>A</Area>
    <Group>G</Group>
  </ChatModePrefixes>
  <BaseColor>white</BaseColor>
</ChatTagPluginConfiguration>
```

### Available Format Placeholders

-   `{PLAYER_NAME}` - The player's name.
-   `{CHAT_MODE}` - The player's chat mode.
-   `{MESSAGE}` - The player's chat message.
-   `{PREFIXES}` - The player's prefixes.
-   `{SUFFIXES}` - The player's suffixes.
