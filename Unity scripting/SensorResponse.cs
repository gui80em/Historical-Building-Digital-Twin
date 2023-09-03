using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorResponse
{
  public Channel channel { get; set; }
  public List<Feeds> feeds { get; set; }
}


public class Channel
{
  public string id { get; set; }
  public string field1 { get; set; }
  public string field2 { get; set; }
  public string field3 { get; set; }
  public string field4 { get; set; }
  public string last_entry_id { get; set; }
}

public class Feeds
{
  public string entry_id {get; set; }
  public string field1 { get; set; }
  public string field2 { get; set; }
  public string field3 { get; set; }
  public string field4 { get; set; }
}
